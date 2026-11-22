using System.Text;
using System.Text.RegularExpressions;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Web.Assistant;

public sealed record IndicatorSuggestion(int IndicatorId, int? Level, string Reason);

public sealed record IndicatorSuggestions(bool IsAvailable, IReadOnlyList<IndicatorSuggestion> Items)
{
    public static IndicatorSuggestions Unavailable { get; } = new(false, []);

    public IndicatorSuggestion? For(int indicatorId) => Items.FirstOrDefault(i => i.IndicatorId == indicatorId);
}

public sealed partial class IndicatorSuggester(
    [FromKeyedServices(NoteCriterionClassifier.ClientKey)] IChatClient chatClient,
    ILogger<IndicatorSuggester> logger)
{
    private const string NoBasis = "BRAK";

    private const string Instructions =
        "Jesteś asystentem przełożonego w służbie cywilnej. Przełożony ocenia pracownika według modelu kompetencji. " +
        "Każdy wskaźnik behawioralny ma pięć opisów zachowań, od 1 (znacznie poniżej oczekiwań) do 5 (znacznie powyżej oczekiwań). " +
        "Na podstawie notatek przełożonego wskaż dla każdego wskaźnika opis zachowania, który najlepiej odpowiada faktom z notatek. " +
        $"Jeśli żadna notatka nie dotyczy wskaźnika, odpowiedz {NoBasis}. Opieraj się tylko na faktach z notatek, niczego nie dopowiadaj. " +
        "To jest tylko podpowiedź dla przełożonego: nie ustalasz oceny kryterium, decyzję podejmuje przełożony. " +
        "Odpowiedz wyłącznie liniami w formacie, po jednej na wskaźnik, bez innego tekstu:\n" +
        $"W<numer wskaźnika>: <1-5 albo {NoBasis}> | <jedno krótkie zdanie uzasadnienia z odwołaniem do notatki>";

    public async Task<IndicatorSuggestions> SuggestAsync(
        Criterion criterion,
        IReadOnlyList<CompetencyIndicator> indicators,
        IReadOnlyList<EvaluationNote> notes,
        CancellationToken ct = default)
    {
        if (notes.Count == 0 || indicators.Count == 0 || chatClient.GetService<MockChatClient>() is not null)
        {
            return IndicatorSuggestions.Unavailable;
        }

        try
        {
            var response = await chatClient.GetResponseAsync(
                [new ChatMessage(ChatRole.User, BuildPrompt(criterion, indicators, notes))],
                new ChatOptions { Instructions = Instructions },
                ct);
            var items = Parse(response.Text, indicators);
            if (items.Count > 0)
            {
                return new IndicatorSuggestions(true, items);
            }
            logger.LogWarning("Model zwrócił podpowiedzi w nieznanym formacie: {Answer}", response.Text);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogWarning(ex, "Nie udało się przygotować podpowiedzi wskaźników.");
        }

        return IndicatorSuggestions.Unavailable;
    }

    public static IReadOnlyList<IndicatorSuggestion> Parse(string? answer, IReadOnlyList<CompetencyIndicator> indicators)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            return [];
        }

        var byNumber = indicators.ToDictionary(i => i.Number);
        var items = new List<IndicatorSuggestion>();
        foreach (var line in answer.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var match = SuggestionLine().Match(line);
            if (!match.Success
                || !byNumber.TryGetValue(int.Parse(match.Groups["number"].Value), out var indicator)
                || items.Any(i => i.IndicatorId == indicator.Id))
            {
                continue;
            }

            int? level = match.Groups["level"].Value.Equals(NoBasis, StringComparison.OrdinalIgnoreCase)
                ? null
                : int.Parse(match.Groups["level"].Value);
            items.Add(new IndicatorSuggestion(indicator.Id, level, match.Groups["reason"].Value.Trim()));
        }
        return items;
    }

    private static string BuildPrompt(Criterion criterion, IReadOnlyList<CompetencyIndicator> indicators, IReadOnlyList<EvaluationNote> notes)
    {
        var prompt = new StringBuilder($"Kryterium: {EvaluationCriteria.Name(criterion)}\n\nWskaźniki i opisy zachowań:\n");
        foreach (var indicator in indicators.OrderBy(i => i.Number))
        {
            prompt.Append($"W{indicator.Number}: {indicator.Text}\n");
            foreach (var behavior in indicator.Behaviors.OrderBy(b => b.Level))
            {
                prompt.Append($"  {behavior.Level} ({CompetencyLevels.Name(behavior.Level)}): {behavior.Description}\n");
            }
        }

        prompt.Append("\nNotatki przełożonego o tym kryterium:\n");
        foreach (var note in notes.OrderBy(n => n.CreatedAt))
        {
            prompt.Append($"<notatka data=\"{note.CreatedAt:yyyy-MM-dd}\" wydzwiek=\"{note.Tone}\">\n{note.Text}\n</notatka>\n");
        }
        return prompt.ToString();
    }

    [GeneratedRegex(@"^\W*W\s*(?<number>\d+)\s*[:.\-]\s*(?<level>[1-5]|BRAK)\b\s*[|\-–:]?\s*(?<reason>.*)$", RegexOptions.IgnoreCase)]
    private static partial Regex SuggestionLine();
}
