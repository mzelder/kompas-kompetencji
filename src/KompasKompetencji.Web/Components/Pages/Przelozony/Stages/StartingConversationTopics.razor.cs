using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class StartingConversationTopics
{
    private static readonly IReadOnlyList<ConversationTopic> Topics =
    [
        new("work", "1. Obowiązki", "Główne zadania na stanowisku i to, za co pracownik odpowiada.", ShowsJobTasks: true),
        new("checklist", "2. Kryteria oceny", $"Kryteria obowiązkowe z arkusza i do {EvaluationCriteria.MaxAdditional} dodatkowych. Wybierz je w części I arkusza poniżej."),
        new("target", "3. Oczekiwania", "Co w praktyce oznacza dobre spełnianie każdego kryterium."),
        new("flag", "4. Cele (opcjonalnie)", "Cele i sposób ich realizacji, jeśli uzasadnia je charakter stanowiska."),
    ];

    private bool tasksExpanded;
    private bool collapsed = true;

    [Parameter] public JobDescription? JobDescription { get; set; }

    private void ToggleCollapsed() => collapsed = !collapsed;

    private void OnHeadingKeyDown(KeyboardEventArgs args)
    {
        if (args.Key is "Enter" or " ")
        {
            ToggleCollapsed();
        }
    }

    private sealed record ConversationTopic(string Icon, string Title, string Description, bool ShowsJobTasks = false);
}
