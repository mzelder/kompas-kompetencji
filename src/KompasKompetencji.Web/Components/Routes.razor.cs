using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components;

public partial class Routes
{
    [Inject] private ThemeService ThemeService { get; set; } = default!;

    protected override void OnInitialized()
    {
        ThemeService.SetTheme(new ThemeOptions { Theme = AppTheme.Name, TriggerChange = false });
    }
}
