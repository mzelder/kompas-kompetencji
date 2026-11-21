using System.Text.RegularExpressions;

namespace KompasKompetencji.Web.Assistant.KnowledgeBase;

public static class Synonyms
{
    private static readonly (Regex Trigger, string Expansion)[] Rules =
    [
        (new(@"\bodwol|\bnie zgadza|\bniezgod|\bzaskarz|\bkwestion"), "sprzeciw sprzeciwu"),
        (new(@"\biprz\b|\bplan\w* rozwoju"), "indywidualnego programu rozwoju zawodowego"),
        (new(@"\barkusz\w*\s+c\b"), "niekierujacy zespolem arkusz"),
        (new(@"\barkusz\w*\s+b\b"), "kierujacy zespolem arkusz"),
        (new(@"\bjak czesto\b|\bco ile\b"), "24 miesiace"),
        (new(@"\bkiedy (trzeba |nalezy |musze |powinien\w* )?(dokon|sporzadz|przeprowadz|zrobi|robi)|\bw jakim terminie\b|\bdo kiedy\b"), "termin dokonuje"),
        (new(@"\bco (sie )?dzieje\b|\bskutk|\bkonsekwencj|\bnastepstw|\bgrozi"), "konsekwencje"),
        (new(@"\bskal\w*|\bpoziom\w* ocen"), "skali"),
        (new(@"\brozmow\w*"), "rozmowa rozmowe rozmowy"),
        (new(@"\bomow\w*|\bomawia\w*"), "omawia"),
    ];

    public static IEnumerable<string> Expand(string normalizedQuery) =>
        Rules.Where(r => r.Trigger.IsMatch(normalizedQuery))
            .SelectMany(r => SearchTokenizer.Words(r.Expansion));
}
