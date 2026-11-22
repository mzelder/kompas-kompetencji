using KompasKompetencji.Web.Data;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class CriterionNotesDialog
{
    [Inject] private DialogService DialogService { get; set; } = default!;

    [Parameter, EditorRequired] public IReadOnlyList<EvaluationNote> Notes { get; set; } = [];

    private static string ToneIcon(NoteTone tone) => tone switch
    {
        NoteTone.Positive => "thumb_up",
        NoteTone.Negative => "thumb_down",
        _ => "remove",
    };

    private static string ToneColor(NoteTone tone) => tone switch
    {
        NoteTone.Positive => Colors.Success,
        NoteTone.Negative => Colors.Danger,
        _ => Colors.Base400,
    };
}
