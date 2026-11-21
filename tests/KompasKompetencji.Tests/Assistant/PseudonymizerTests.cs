using System.Runtime.CompilerServices;
using KompasKompetencji.Web.Assistant.Privacy;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Tests.Assistant;

public class PseudonymizerTests
{
    private static readonly PersonName[] DemoPeople =
    [
        new(1, "Anna", "Kowalska"),
        new(2, "Jan", "Nowak"),
        new(3, "Ewa", "Wiśniewska"),
        new(4, "Tomasz", "Zieliński"),
        new(5, "Marek", "Lewandowski"),
        new(6, "Magdalena", "Dąbrowska"),
        new(7, "Katarzyna", "Wójcik"),
        new(8, "Piotr", "Kamiński"),
    ];

    private readonly Pseudonymizer pseudonymizer = new(DemoPeople);

    [Theory]
    [InlineData("Jak przygotować rozmowę z Anną Kowalską?", "Jak przygotować rozmowę z Pracownik A?")]
    [InlineData("Co z Kowalską i Janem Nowakiem?", "Co z Pracownik A i Pracownik B?")]
    [InlineData("Ocena Marka Lewandowskiego", "Ocena Pracownik E")]
    [InlineData("Piotrze, Annie i Ewie", "Pracownik H, Pracownik A i Pracownik C")]
    [InlineData("zielińskiego i KOWALSKIEJ", "Pracownik D i Pracownik A")]
    [InlineData("Wiśniewska Ewa", "Pracownik C")]
    public void ReplacesNamesInDifferentForms(string text, string expected)
    {
        Assert.Equal(expected, pseudonymizer.Pseudonymize(text));
    }

    [Theory]
    [InlineData("Ewaluacja pracy i nowa ankieta")]
    [InlineData("Ile dni ma pracownik na sprzeciw od oceny okresowej?")]
    [InlineData("Zielony arkusz i janowa łąka")]
    public void LeavesOtherWordsUnchanged(string text)
    {
        Assert.Equal(text, pseudonymizer.Pseudonymize(text));
    }

    [Theory]
    [InlineData("Pracownik A dobrze współpracuje.", "Anna Kowalska dobrze współpracuje.")]
    [InlineData("Porozmawiaj z Pracownikiem B i Pracownika H.", "Porozmawiaj z Jan Nowak i Piotr Kamiński.")]
    [InlineData("Pracownik Z nie istnieje.", "Pracownik Z nie istnieje.")]
    public void RestoresNames(string text, string expected)
    {
        Assert.Equal(expected, pseudonymizer.Restore(text));
    }

    [Fact]
    public async Task ModelSeesPseudonymsAndUserSeesNames()
    {
        var model = new ScriptedChatClient(["Rozmowę z Pracow", "nikiem A", " zacznij", " od pochwały", " Pracownik", " A."]);
        var client = new PseudonymizingChatClient(model, _ => Task.FromResult<IReadOnlyList<PersonName>>(DemoPeople));

        var text = string.Empty;
        await foreach (var update in client.GetStreamingResponseAsync([new ChatMessage(ChatRole.User, "Jak przygotować rozmowę z Anną Kowalską?")]))
            text += update.Text;

        Assert.Equal("Jak przygotować rozmowę z Pracownik A?", model.LastQuestion);
        Assert.Equal("Rozmowę z Anna Kowalska zacznij od pochwały Anna Kowalska.", text);
    }

    private sealed class ScriptedChatClient(string[] chunks) : IChatClient
    {
        public string? LastQuestion { get; private set; }

        public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default) =>
            GetStreamingResponseAsync(messages, options, cancellationToken).ToChatResponseAsync(cancellationToken);

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            LastQuestion = messages.Last().Text;
            foreach (var chunk in chunks)
            {
                await Task.Yield();
                yield return new ChatResponseUpdate(ChatRole.Assistant, chunk);
            }
        }

        public object? GetService(Type serviceType, object? serviceKey = null) => null;

        public void Dispose()
        {
        }
    }
}
