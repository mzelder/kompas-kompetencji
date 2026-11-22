using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class StartingConversationTopics
{
    private static readonly IReadOnlyList<ConversationTopic> Topics =
    [
        new("work", "1. Obowiązki", "Główne zadania na stanowisku i to, za co pracownik odpowiada.", ShowsJobTasks: true),
        new("checklist", "2. Kryteria oceny", $"Kryteria obowiązkowe z arkusza i do {EvaluationCriteria.MaxAdditional} dodatkowych. Wybierz je w arkuszu obok."),
        new("target", "3. Oczekiwania", "Co w praktyce oznacza dobre spełnianie każdego kryterium."),
        new("flag", "4. Cele (opcjonalnie)", "Cele i sposób ich realizacji, jeśli uzasadnia je charakter stanowiska."),
    ];

    private bool tasksExpanded;

    [Parameter] public JobDescription? JobDescription { get; set; }

    private sealed record ConversationTopic(string Icon, string Title, string Description, bool ShowsJobTasks = false);
}
