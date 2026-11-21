using System.Text.RegularExpressions;
using KompasKompetencji.Web.Text;

namespace KompasKompetencji.Web.Assistant;

public static class EvaluationLevelGuard
{
    public const string Refusal =
        "Nie mogę zdecydować o poziomie oceny pracownika ani go zaproponować. Sugerowany poziom liczy jawna reguła " +
        "na podstawie zachowań, które wybierzesz, a ostateczną decyzję zawsze podejmujesz Ty.\n\n" +
        "Mogę za to wyjaśnić, co oznaczają poziomy skali, albo pomóc uporządkować przykłady zachowań " +
        "i argumenty do uzasadnienia oceny.";

    private const string Assign = @"(dac|dam|dalbys|dalabys|wystawic|wystawie|wystawilbys|przyznac|przyznam|przyznalbys|postawic|postawie)";
    private const string Rating = @"(poziom\w*|ocen\w*|punkt\w*)";

    private static readonly Regex[] Patterns =
    [
        new($@"\b(jak\w*|ile)\s+{Rating}\s+(\w+\s+)?{Assign}\b"),
        new($@"\b(moge|powinienem|powinnam|mam|musze|chce|warto)\s+(\w+\s+)?{Assign}\b"),
        new($@"\b{Assign}\s+(\w+\s+){{0,2}}(\d+|jeden|trzy|piec|siedem|dziewiec)\s+punkt\w*"),
        new(@"\bjak\w*\s+(ocen\w*|poziom\w*)\s+(\w+\s+)?(powinien|powinna|powinno|zasluguje|zasluzyl\w*|dostanie|dostac)\b"),
        new(@"\bna jakim poziomie\s+(\w+\s+)?(ocenil\w*|ocenic|ocenisz|powinien|powinna|jest\s+(moj|moja|ten|ta|jego|jej))\b"),
        new(@"\bocen\s+(go|ja|jego|jej|mojego|moja|tego pracownika|te osobe)\b"),
    ];

    public static bool IsLevelRequest(string? question)
    {
        if (string.IsNullOrWhiteSpace(question))
            return false;

        var normalized = TextNormalizer.Normalize(question);
        return Patterns.Any(p => p.IsMatch(normalized));
    }
}
