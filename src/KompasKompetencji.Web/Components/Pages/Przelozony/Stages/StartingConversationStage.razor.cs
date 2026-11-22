using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class StartingConversationStage
{
    [Inject] private EvaluationService EvaluationService { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;

    [Parameter, EditorRequired] public EmployeeEvaluationView View { get; set; } = default!;

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter] public bool ReadOnly { get; set; }

    [Parameter] public EventCallback OnChanged { get; set; }

    private IEnumerable<Criterion> additionalCriteria = [];
    private string? expectations;
    private string? goals;
    private bool saving;
    private bool markingHeld;
    private bool confirming;
    private DateOnly? acquaintedOn;
    private string? errorMessage;

    private DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    private Conversation? Conversation => View.StartingConversation;

    private bool IsHeld => Conversation?.Status == ConversationStatus.Held;

    private bool IsConversationPastDue => ConversationRules.IsPastDue(Conversation?.At, Conversation?.Status, DateTime.Now);

    private bool IsDeadlineOverdue => StartingConversationRules.IsOverdue(View.CycleStart, DateTime.Now);

    private string ConversationSubtitle => Conversation is null
        ? "Termin rozmowy nie jest wyznaczony"
        : EvaluationText.Date(Conversation.At);

    private EvaluationSheet Sheet => View.OpenEvaluation?.Sheet ?? View.Task.Sheet;

    private bool IsReadyToConfirm => View.StartingConversationStep == StartingConversationStep.ReadyToConfirm;

    private DateOnly ConversationDay => Conversation is null ? Today : DateOnly.FromDateTime(Conversation.At);

    private string PartISubtitle => View.StartingConversationStep switch
    {
        StartingConversationStep.Confirmed => "Część I zatwierdzona po rozmowie rozpoczynającej.",
        StartingConversationStep.ReadyToConfirm => "Uzupełnij ustalenia z rozmowy i zatwierdź",
        _ => "Propozycja do omówienia z pracownikiem",
    };

    protected override void OnInitialized()
    {
        additionalCriteria = View.AdditionalCriteria.ToList();
        expectations = View.Expectations;
        goals = View.Goals;
        acquaintedOn = IsReadyToConfirm ? ConversationDay : null;
    }

    private bool IsOptionDisabled(Criterion criterion) =>
        additionalCriteria.Count() >= EvaluationCriteria.MaxAdditional && !additionalCriteria.Contains(criterion);

    private async Task SaveDraftAsync()
    {
        errorMessage = null;
        saving = true;
        var result = await EvaluationService.SavePartIDraftAsync(
            SupervisorId, View.Employee.Id, additionalCriteria.ToList(), expectations, goals);
        saving = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Propozycja zapisana", View.Employee.FullName);
        if (Conversation is null)
        {
            await ScheduleConversationAsync();
        }
        await OnChanged.InvokeAsync();
    }

    private async Task ConfirmPartIAsync()
    {
        errorMessage = null;
        if (string.IsNullOrWhiteSpace(expectations))
        {
            errorMessage = "Wpisz oczekiwania omówione z pracownikiem.";
            return;
        }

        if (acquaintedOn is not DateOnly acquaintedDay || acquaintedDay < ConversationDay || acquaintedDay > Today)
        {
            errorMessage = $"Data zapoznania musi przypadać między dniem rozmowy rozpoczynającej ({EvaluationText.Date(ConversationDay)}) a dzisiejszym dniem.";
            return;
        }

        var confirmed = await DialogService.Confirm(
            "Po zatwierdzeniu kryteriów nie można zmienić. Pracownik potwierdził rozmowę i zapoznanie z kryteriami oraz terminem oceny?",
            "Zatwierdź część I",
            new ConfirmOptions { OkButtonText = "Zatwierdź", CancelButtonText = "Anuluj" });
        if (confirmed != true)
        {
            return;
        }

        confirming = true;
        var result = await EvaluationService.ConfirmPartIAsync(
            SupervisorId, View.Employee.Id, additionalCriteria.ToList(), expectations, goals, acquaintedDay, DateTime.Now);
        confirming = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Część I zatwierdzona", View.Employee.FullName);
        await OnChanged.InvokeAsync();
    }

    private async Task EditConversationAsync()
    {
        if (await ScheduleConversationAsync())
        {
            await OnChanged.InvokeAsync();
        }
    }

    private async Task<bool> ScheduleConversationAsync()
    {
        if (!await ConversationDialog.OpenAsync(DialogService, View.Task, ConversationType.Starting, SupervisorId, Today))
        {
            return false;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Zapisano termin rozmowy", $"{View.Employee.FullName} · {EvaluationText.ConversationTypeName(ConversationType.Starting)}");
        return true;
    }

    private async Task MarkHeldAsync()
    {
        if (Conversation is null)
        {
            return;
        }

        markingHeld = true;
        var result = await EvaluationService.SaveConversationAsync(
            SupervisorId, View.Employee.Id, ConversationType.Starting, Conversation.At, ConversationStatus.Held, DateTime.Now);
        markingHeld = false;

        if (!result.Succeeded)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Nie udało się zapisać", result.Error ?? string.Empty);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Rozmowa oznaczona jako odbyta", View.Employee.FullName);
        await OnChanged.InvokeAsync();
    }
}
