using KompasKompetencji.Web.Data;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class StartingConversationTopics
{
    private static readonly IReadOnlyList<string> Topics =
    [
        "Główne obowiązki z opisu stanowiska",
        "Kryteria oceny: obowiązkowe i dodatkowe",
        "Oczekiwania co do spełniania kryteriów",
        "Cele i sposób ich realizacji, jeśli uzasadnia je charakter stanowiska",
    ];

    [Parameter] public JobDescription? JobDescription { get; set; }
}
