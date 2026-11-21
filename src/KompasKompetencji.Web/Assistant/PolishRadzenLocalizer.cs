using System.Globalization;
using Radzen;

namespace KompasKompetencji.Web.Assistant;

public sealed class PolishRadzenLocalizer : ILocalizer
{
    private static readonly Dictionary<string, string> Strings = new()
    {
        ["AIChat_Sources"] = "Źródła",
        ["AIChat_SendTitle"] = "Wyślij",
        ["AIChat_StopTitle"] = "Zatrzymaj",
        ["AIChat_ClearChatTitle"] = "Wyczyść rozmowę",
        ["AIChat_ScrollToEndTitle"] = "Przewiń na dół",
    };

    public string? Get(string key, CultureInfo culture) => Strings.GetValueOrDefault(key);
}
