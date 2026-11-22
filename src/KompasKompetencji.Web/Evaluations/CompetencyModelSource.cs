using System.Text.Json;
using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations;

public static class CompetencyModelSource
{
    public const string ResourceName = "competency-model.json";

    private sealed record ModelFile(string Source, string[] Levels, CompetencyFile[] Competencies);

    private sealed record CompetencyFile(string Criterion, string Name, IndicatorFile[] Indicators);

    private sealed record IndicatorFile(string Text, string[] Levels);

    private static readonly Lazy<(IReadOnlyList<CompetencyIndicator> Indicators, IReadOnlyList<IndicatorBehavior> Behaviors)> Seed = new(Load);

    public static IReadOnlyList<CompetencyIndicator> Indicators => Seed.Value.Indicators;

    public static IReadOnlyList<IndicatorBehavior> Behaviors => Seed.Value.Behaviors;

    private static (IReadOnlyList<CompetencyIndicator>, IReadOnlyList<IndicatorBehavior>) Load()
    {
        using var stream = typeof(CompetencyModelSource).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Brakuje zasobu {ResourceName}. Uruchom competency-model/build_competency_model.py.");
        var model = JsonSerializer.Deserialize<ModelFile>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new InvalidOperationException($"Nie udało się odczytać {ResourceName}.");

        var indicators = new List<CompetencyIndicator>();
        var behaviors = new List<IndicatorBehavior>();
        foreach (var competency in model.Competencies)
        {
            var criterion = Enum.Parse<Criterion>(competency.Criterion);
            for (var i = 0; i < competency.Indicators.Length; i++)
            {
                var source = competency.Indicators[i];
                var id = indicators.Count + 1;
                indicators.Add(new CompetencyIndicator { Id = id, Criterion = criterion, Number = i + 1, Text = source.Text });
                behaviors.AddRange(source.Levels.Select((description, index) =>
                    new IndicatorBehavior { IndicatorId = id, Level = index + 1, Description = description }));
            }
        }
        return (indicators, behaviors);
    }
}
