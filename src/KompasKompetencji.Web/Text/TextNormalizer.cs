using System.Globalization;
using System.Text;

namespace KompasKompetencji.Web.Text;

public static class TextNormalizer
{
    public static string Normalize(string text)
    {
        var decomposed = text.ToLowerInvariant().Replace('ł', 'l').Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(decomposed.Length);
        foreach (var c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                builder.Append(c);
        }
        return builder.ToString();
    }
}
