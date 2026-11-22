using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Evaluations.Pdf;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class CompletionStage
{
    [Inject] private EvaluationService EvaluationService { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Parameter, EditorRequired] public EmployeeEvaluationView View { get; set; } = default!;

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter] public EventCallback OnChanged { get; set; }

    private DateOnly? preparedOn;
    private DateOnly? acquaintedOn;
    private bool savingPreparedOn;
    private bool closing;
    private string? preparationError;
    private string? closeError;

    private CompletionView Completion => View.Completion!;

    private bool IsPrepared => Completion.Step == CompletionStep.AwaitingAcquaintance;

    private bool CanPrepare => Completion.AssessmentConversationDay is not null;

    private string PreparationSubtitle => Completion.PreparedOn is DateOnly day
        ? $"Ocena sporządzona {EvaluationText.Date(day)}."
        : "Wpisz datę sporządzenia oceny i pobierz arkusz do podpisu.";

    private string PreparedOnHelp => Completion.AssessmentConversationDay is DateOnly conversationDay
        ? $"Data podpisu oceniającego. Kończy okres oceny i rozpoczyna nowy cykl. Nie wcześniej niż rozmowa oceniająca ({EvaluationText.Date(conversationDay)})."
        : "Data podpisu oceniającego. Kończy okres oceny i rozpoczyna nowy cykl.";

    private string PeriodText => preparedOn is DateOnly day
        ? $"{EvaluationText.Date(View.OpenEvaluation!.PeriodStart)} – {EvaluationText.Date(day)}"
        : $"od {EvaluationText.Date(View.OpenEvaluation!.PeriodStart)}";

    protected override void OnInitialized()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        preparedOn = Completion.PreparedOn ?? Clamp(today, Completion.PreparedOnRange);
        acquaintedOn = Clamp(today, Completion.AcquaintedOnRange);
    }

    private async Task SavePreparedOnAsync()
    {
        preparationError = null;
        if (preparedOn is not DateOnly day || !Completion.PreparedOnRange.Contains(day))
        {
            preparationError = $"Data sporządzenia oceny musi przypadać między dniem rozmowy oceniającej ({EvaluationText.Date(Completion.PreparedOnRange.Min)}) a dzisiejszym dniem.";
            return;
        }

        savingPreparedOn = true;
        var result = await EvaluationService.SavePreparedOnAsync(SupervisorId, View.Employee.Id, day, DateTime.Now);
        savingPreparedOn = false;

        if (!result.Succeeded)
        {
            preparationError = result.Error;
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Zapisano datę sporządzenia oceny", View.Employee.FullName);
        await OnChanged.InvokeAsync();
    }

    private async Task CloseEvaluationAsync()
    {
        closeError = null;
        if (acquaintedOn is not DateOnly day || !Completion.AcquaintedOnRange.Contains(day))
        {
            closeError = $"Data zapoznania pracownika z oceną musi przypadać między datą sporządzenia oceny ({EvaluationText.Date(Completion.AcquaintedOnRange.Min)}) a dzisiejszym dniem.";
            return;
        }

        var confirmed = await DialogService.Confirm(
            "Po zamknięciu oceny nie można jej zmienić, a pracownik zaczyna nowy cykl oceny. Pracownik podpisał arkusz?",
            "Zamknij ocenę",
            new ConfirmOptions { OkButtonText = "Zamknij ocenę", CancelButtonText = "Anuluj" });
        if (confirmed != true)
        {
            return;
        }

        closing = true;
        var result = await EvaluationService.CloseEvaluationAsync(SupervisorId, View.Employee.Id, day, DateTime.Now);
        closing = false;

        if (!result.Succeeded)
        {
            closeError = result.Error;
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Ocena zamknięta", View.Employee.FullName);
        await OnChanged.InvokeAsync();
    }

    private void DownloadPdf() =>
        Navigation.NavigateTo(EvaluationPdfEndpoints.SheetUrl(View.OpenEvaluation!.Id), forceLoad: true);

    private static DateOnly Clamp(DateOnly day, AllowedDates range) =>
        day < range.Min ? range.Min : day > range.Max ? range.Max : day;
}
