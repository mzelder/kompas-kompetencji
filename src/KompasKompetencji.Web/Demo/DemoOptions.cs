namespace KompasKompetencji.Web.Demo;

public sealed class DemoOptions
{
    public const string SectionName = "Demo";

    public bool ResetDatabaseOnStartup { get; init; }

    public bool ShowTestAccounts { get; init; }
}
