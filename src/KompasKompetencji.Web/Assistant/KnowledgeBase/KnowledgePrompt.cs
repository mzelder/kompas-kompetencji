using System.Text;
using System.Text.RegularExpressions;

namespace KompasKompetencji.Web.Assistant.KnowledgeBase;

public static partial class KnowledgePrompt
{
    public const string Header = "Fragmenty bazy wiedzy do tego pytania:";
    public const string NoFragments = "Brak fragmentów bazy wiedzy pasujących do tego pytania.";

    public static string Build(IReadOnlyList<KnowledgeSearchResult> results)
    {
        if (results.Count == 0)
            return NoFragments;

        var builder = new StringBuilder(Header);
        for (var i = 0; i < results.Count; i++)
        {
            builder.Append("\n\n[").Append(i + 1).Append("] ").Append(results[i].Chunk.Title).Append('\n');
            builder.Append(results[i].Chunk.Text);
        }
        return builder.ToString();
    }

    public static IReadOnlyList<int> CitedNumbers(string answer, int fragmentCount) =>
        CitationPattern().Matches(answer)
            .SelectMany(m => m.Groups[1].Value.Split([',', ';'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .Select(n => int.TryParse(n, out var number) ? number : 0)
            .Where(n => n >= 1 && n <= fragmentCount)
            .Distinct()
            .Order()
            .ToList();

    public static IReadOnlyList<(string Title, string Text)> Parse(string? instructions)
    {
        if (string.IsNullOrEmpty(instructions))
            return [];

        var start = instructions.IndexOf(Header, StringComparison.Ordinal);
        if (start < 0)
            return [];

        return FragmentPattern().Matches(instructions[(start + Header.Length)..])
            .Select(m => (m.Groups[1].Value.Trim(), m.Groups[2].Value.Trim()))
            .ToList();
    }

    [GeneratedRegex(@"\[(\d+(?:\s*[,;]\s*\d+)*)(?=[\],;\s(])")]
    private static partial Regex CitationPattern();

    [GeneratedRegex(@"\n\n\[\d+\] ([^\n]+)\n(.*?)(?=\n\n\[\d+\] |\z)", RegexOptions.Singleline)]
    private static partial Regex FragmentPattern();
}
