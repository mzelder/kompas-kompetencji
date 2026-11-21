using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class UserAvatar
{
    [Parameter, EditorRequired] public string? Name { get; set; }

    private string Initials
    {
        get
        {
            var words = (Name ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return words.Length switch
            {
                0 => "?",
                1 => words[0][..Math.Min(2, words[0].Length)],
                _ => $"{words[0][0]}{words[^1][0]}",
            };
        }
    }
}
