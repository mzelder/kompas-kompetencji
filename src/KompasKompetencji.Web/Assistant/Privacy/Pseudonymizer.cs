using System.Text;
using System.Text.RegularExpressions;
using KompasKompetencji.Web.Text;

namespace KompasKompetencji.Web.Assistant.Privacy;

public sealed record PersonName(int Id, string FirstName, string LastName)
{
    public string FullName => $"{FirstName} {LastName}";
}

public sealed partial class Pseudonymizer
{
    private const string Prefix = "Pracownik";

    private static readonly string[] Endings =
    [
        "", "a", "e", "i", "o", "u", "y", "ie", "em", "om", "ej", "iej", "owi", "iem", "iego", "iemu", "im",
        "ze", "ego", "emu", "owie", "ow", "ach", "ami",
    ];

    private readonly List<Person> people;
    private readonly Dictionary<string, PersonName> byLabel;

    public Pseudonymizer(IEnumerable<PersonName> names)
    {
        people = names
            .OrderBy(n => n.Id)
            .Select((name, index) => new Person(name, $"{Prefix} {Label(index)}", Stems(name.FirstName), Stems(name.LastName)))
            .ToList();
        byLabel = people.ToDictionary(p => p.Pseudonym[(Prefix.Length + 1)..], p => p.Name);
    }

    public string Pseudonymize(string text)
    {
        var words = WordPattern().Matches(text).ToList();
        if (words.Count == 0 || people.Count == 0)
            return text;

        var result = new StringBuilder();
        var position = 0;
        for (var i = 0; i < words.Count; i++)
        {
            var word = Normalize(words[i].Value);
            var next = i + 1 < words.Count && IsOnlyWhitespace(text, words[i], words[i + 1]) ? Normalize(words[i + 1].Value) : null;

            var pair = next is null ? null : people.FirstOrDefault(p =>
                (Matches(word, p.FirstStems) && Matches(next, p.LastStems)) ||
                (Matches(word, p.LastStems) && Matches(next, p.FirstStems)));
            if (pair is not null)
            {
                result.Append(text, position, words[i].Index - position).Append(pair.Pseudonym);
                position = words[i + 1].Index + words[i + 1].Length;
                i++;
                continue;
            }

            var single = people.FirstOrDefault(p => Matches(word, p.LastStems)) ?? people.FirstOrDefault(p => Matches(word, p.FirstStems));
            if (single is not null)
            {
                result.Append(text, position, words[i].Index - position).Append(single.Pseudonym);
                position = words[i].Index + words[i].Length;
            }
        }

        return result.Append(text, position, text.Length - position).ToString();
    }

    public string Restore(string text) =>
        PseudonymPattern().Replace(text, m => byLabel.TryGetValue(m.Groups[1].Value, out var name) ? name.FullName : m.Value);

    public int SafeLength(string pending)
    {
        var lastBreak = pending.LastIndexOfAny([' ', '\n', '\t', '\r']);
        var safe = lastBreak + 1;
        foreach (Match prefix in PrefixPattern().Matches(pending))
        {
            if (prefix.Index >= safe)
                break;
            var full = PseudonymPattern().Match(pending, prefix.Index);
            var end = full.Success && full.Index == prefix.Index ? full.Index + full.Length : int.MaxValue;
            if (end >= safe)
                return prefix.Index;
        }
        return safe;
    }

    private static bool Matches(string word, IReadOnlyList<string> stems) =>
        stems.Any(stem => word.StartsWith(stem, StringComparison.Ordinal) && Endings.Contains(word[stem.Length..]));

    private static IReadOnlyList<string> Stems(string name)
    {
        var normalized = Normalize(name);
        var stems = new List<string> { normalized };
        if (normalized.Length > 3 && "aeiouy".Contains(normalized[^1]))
            stems.Add(normalized[..^1]);
        if (normalized.EndsWith("ek", StringComparison.Ordinal) && normalized.Length > 4)
            stems.Add(normalized[..^2] + "k");
        if (normalized.Length == 3 && "aeiouy".Contains(normalized[^1]))
            stems.Add(normalized[..^1]);
        return stems;
    }

    private static bool IsOnlyWhitespace(string text, Match first, Match second)
    {
        var start = first.Index + first.Length;
        return text.AsSpan(start, second.Index - start).IsWhiteSpace();
    }

    private static string Normalize(string word) => TextNormalizer.Normalize(word);

    private static string Label(int index)
    {
        var label = string.Empty;
        for (var n = index; n >= 0; n = n / 26 - 1)
            label = (char)('A' + n % 26) + label;
        return label;
    }

    [GeneratedRegex(@"\p{L}+")]
    private static partial Regex WordPattern();

    [GeneratedRegex(@"\bPracownik\w*\s+([A-Z]{1,2})\b")]
    private static partial Regex PseudonymPattern();

    [GeneratedRegex(@"\bPracownik\w*")]
    private static partial Regex PrefixPattern();

    private sealed record Person(PersonName Name, string Pseudonym, IReadOnlyList<string> FirstStems, IReadOnlyList<string> LastStems);
}
