using System.Text.RegularExpressions;
using KompasKompetencji.Web.Text;

namespace KompasKompetencji.Web.Assistant.KnowledgeBase;

public static partial class SearchTokenizer
{
    private static readonly HashSet<string> StopWords =
    [
        "a", "aby", "albo", "ale", "bo", "by", "byc", "co", "czy", "dla", "do", "gdy", "ich", "ile", "i", "jak",
        "jaka", "jaki", "jakie", "jakich", "jakim", "jej", "jego", "jesli", "jest", "juz", "lub", "ma", "mam",
        "mi", "mnie", "moge", "moj", "moja", "moze", "mozna", "na", "nie", "o", "od", "oraz", "po", "przez",
        "przy", "sa", "sie", "ta", "tak", "te", "tego", "tej", "ten", "to", "tez", "tylko", "u", "w", "we",
        "z", "za", "ze", "ktora", "ktore", "ktory", "ktorych", "ktorym", "kto", "kiedy", "gdzie", "dlaczego",
        "prosze", "powiedz", "jakiej", "jakiego",
    ];

    public static IReadOnlyList<string> Tokenize(string text) =>
        Words(TextNormalizer.Normalize(text));

    public static IReadOnlyList<string> Words(string normalizedText) =>
        WordPattern().Matches(normalizedText)
            .Select(m => m.Value)
            .Where(w => (w.Length > 1 || char.IsDigit(w[0])) && !StopWords.Contains(w))
            .Select(Stem)
            .ToList();

    public const int StemLength = 7;

    public static string Stem(string word) =>
        word.Length > StemLength && !char.IsDigit(word[0]) ? word[..StemLength] : word;

    [GeneratedRegex(@"[\p{L}\p{N}]+")]
    private static partial Regex WordPattern();
}
