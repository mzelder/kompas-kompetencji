using KompasKompetencji.Web.Auth;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class Dashboard
{
    [Inject] private UserService Users { get; set; } = default!;
    [Inject] private EvaluationService EvaluationService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;

    [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;

    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Today);
    private SupervisorOverview? overview;
    private int supervisorId;
    private bool accountNotLinked;
    private string? loadError;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var userId = (await AuthState).User.GetUserId();
            var user = userId is null ? null : await Users.GetByIdAsync(userId.Value);
            if (user?.EmployeeId is not int employeeId)
            {
                accountNotLinked = true;
                return;
            }

            supervisorId = employeeId;
            await LoadOverviewAsync();
        }
        catch (Exception ex)
        {
            loadError = ex.Message;
        }
    }

    private async Task LoadOverviewAsync()
    {
        overview = await EvaluationService.GetSupervisorOverviewAsync(supervisorId, DateTime.Now);
    }

    private async Task EditConversationAsync(EvaluationTask task)
    {
        if (await ConversationDialog.OpenAsync(DialogService, task, supervisorId, today))
        {
            await LoadOverviewAsync();
            NotificationService.Notify(NotificationSeverity.Success, "Zapisano termin rozmowy", task.Employee.FullName);
        }
    }

    private async Task MarkHeldAsync(EvaluationTask task)
    {
        if (task.ConversationAt is not DateTime conversationAt)
        {
            return;
        }

        var result = await EvaluationService.SaveConversationAsync(
            supervisorId, task.Employee.Id, conversationAt, ConversationStatus.Held, DateTime.Now);
        if (!result.Succeeded)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Nie udało się zapisać", result.Error ?? string.Empty);
            return;
        }

        await LoadOverviewAsync();
        NotificationService.Notify(NotificationSeverity.Success, "Rozmowa oznaczona jako odbyta", task.Employee.FullName);
    }

    private void OnSlotRender(SchedulerSlotRenderEventArgs args)
    {
        if (DateOnly.FromDateTime(args.Start) == today)
        {
            args.Attributes["data-today"] = "true";
        }

        if (args.View is RadzenMonthView)
        {
            var shownMonth = args.View.StartDate + (args.View.EndDate - args.View.StartDate) / 2;
            if (args.Start.Month != shownMonth.Month || args.Start.Year != shownMonth.Year)
            {
                args.Attributes["data-other-month"] = "true";
            }
        }
    }

    private void OnConversationRender(SchedulerAppointmentRenderEventArgs<EvaluationConversation> args)
    {
        args.Attributes["style"] = args.Data switch
        {
            { Status: ConversationStatus.Held } => "background-color: var(--rz-base-600); color: var(--rz-text-color);",
            { } conversation when ConversationRules.IsPastDue(conversation.Start, conversation.Status, DateTime.Now)
                => "outline: 2px solid var(--rz-warning); outline-offset: -2px;",
            _ => "",
        };
    }

    private async Task OnConversationSelect(SchedulerAppointmentSelectEventArgs<EvaluationConversation> args)
    {
        if (args.Data is not EvaluationConversation conversation)
        {
            return;
        }

        if (conversation.IsEditable && overview?.TaskFor(conversation.Employee.Id) is EvaluationTask task)
        {
            await EditConversationAsync(task);
            return;
        }

        await DialogService.Alert(
            $"{EvaluationText.Date(conversation.Start)} – {conversation.End:HH:mm}. Status rozmowy: {EvaluationText.Conversation(conversation.Status).ToLowerInvariant()}.",
            $"Rozmowa okresowa: {conversation.Employee.FullName}",
            new AlertOptions { OkButtonText = "Zamknij" });
    }
}
