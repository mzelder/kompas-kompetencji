using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Web.Assistant;

public sealed class MockChatClient : IChatClient
{
    private const string Statute = "Ustawa o służbie cywilnej";
    private const string PeriodicRegulation = "Rozporządzenie w sprawie ocen okresowych (2016)";
    private const string FirstRegulation = "Rozporządzenie w sprawie pierwszej oceny (2015)";
    private const string PeriodicGuide = "Poradnik: ocena okresowa w służbie cywilnej (KPRM)";
    private const string SheetC = "Arkusz oceny okresowej C";

    private static readonly CannedAnswer[] Answers =
    [
        new(["jaki poziom", "jaka ocene", "jaka ocena", "wystaw", "ile punktow dac", "oceni pracownika"],
            "Nie mogę zdecydować o poziomie oceny pracownika. Sugerowany poziom kompetencji liczy kreator " +
            "na podstawie zachowań, które wybierzesz, a ostateczną decyzję zawsze podejmujesz Ty.\n\n" +
            "Mogę za to pomóc uporządkować przykłady zachowań i argumenty do uzasadnienia oceny.",
            []),
        new(["sprzeciw", "odwola", "nie zgadza sie z ocena"],
            "Od oceny okresowej przysługuje sprzeciw. Oceniany może go wnieść do dyrektora generalnego urzędu " +
            "w terminie **7 dni** od dnia zapoznania się z oceną [1].\n\n" +
            "Co warto zrobić jako przełożony:\n\n" +
            "- zadbaj, aby data zapoznania się z oceną była udokumentowana w arkuszu,\n" +
            "- przygotuj uzasadnienie oceny oparte na przykładach zachowań z notatek [2].",
            [
                new($"{Statute}, art. 83", "Ocenianemu przysługuje sprzeciw do dyrektora generalnego urzędu, wnoszony w terminie 7 dni od dnia zapoznania się z oceną okresową."),
                new($"{PeriodicGuide}, „Uzasadnienie oceny”", "Uzasadnienie powinno odwoływać się do konkretnych, obserwowanych zachowań ocenianego w okresie podlegającym ocenie."),
            ]),
        new(["jak czesto", "co ile", "24 miesi", "kiedy ocena okresowa", "termin oceny okresowej"],
            "Członek korpusu służby cywilnej podlega ocenie okresowej **co 24 miesiące**. Ocenę sporządza " +
            "bezpośredni przełożony [1].\n\n" +
            "Szczegółowy tryb, w tym ustalenie kryteriów na początku okresu oceny, określa rozporządzenie [2].",
            [
                new($"{Statute}, art. 81 ust. 1", "Członek korpusu służby cywilnej podlega ocenie okresowej dokonywanej przez bezpośredniego przełożonego co 24 miesiące."),
                new($"{PeriodicRegulation}, § 2–3", "Przepisy określające tryb ustalania kryteriów oceny i sporządzania oceny okresowej."),
            ]),
        new(["kryteri", "arkusz c", "arkusz b", "obowiazkow", "dodatkow"],
            "W arkuszu C (osoby niekierujące zespołem) obowiązkowe są kryteria [1]:\n\n" +
            "- wykorzystywanie wiedzy i doświadczenia zawodowego,\n" +
            "- rzetelność,\n" +
            "- organizacja pracy,\n" +
            "- współpraca,\n" +
            "- komunikacja.\n\n" +
            "Dodatkowo możesz wybrać **do 3 kryteriów** z modelu kompetencji, dopasowanych do stanowiska, " +
            "np. na podstawie opisu stanowiska pracy [2].",
            [
                new($"{SheetC}, Część I", "Kryteria obowiązkowe: wykorzystywanie wiedzy i doświadczenia zawodowego, rzetelność, organizacja pracy, współpraca, komunikacja."),
                new($"{PeriodicRegulation}, § 3", "Oceniający wybiera kryteria dodatkowe właściwe dla stanowiska zajmowanego przez ocenianego."),
            ]),
        new(["pierwsza ocen", "pierwszej ocen"],
            "Pierwsza ocena dotyczy osoby, która po raz pierwszy podjęła pracę w służbie cywilnej. " +
            "Arkusz pierwszej oceny ma stały zestaw elementów i trzystopniową skalę: poniżej oczekiwań, " +
            "na poziomie oczekiwań, powyżej oczekiwań [1].\n\n" +
            "W przeciwieństwie do oceny okresowej nie dobierasz tu kryteriów dodatkowych [1].",
            [
                new($"{FirstRegulation}, § 2 i arkusz", "Arkusz pierwszej oceny obejmuje stały zestaw elementów oceny oraz skalę poziomów ich spełniania."),
            ]),
        new(["rozmow", "pytani", "przygotowa", "omowi"],
            "Propozycje pytań do rozmowy oceniającej:\n\n" +
            "- Które zadanie z ostatniego okresu uważasz za swój największy sukces i dlaczego?\n" +
            "- W jakiej sytuacji współpraca z zespołem była dla Ciebie trudna? Co wtedy zrobiłeś?\n" +
            "- Czego potrzebujesz, żeby lepiej organizować swoją pracę?\n" +
            "- Jakie umiejętności chcesz rozwinąć w kolejnym okresie?\n\n" +
            "Zacznij od mocnych stron, a przy obszarach do poprawy odwołuj się do konkretnych przykładów zachowań [1].",
            [
                new($"{PeriodicGuide}, „Rozmowa oceniająca”", "Rozmowa powinna opierać się na faktach i przykładach zachowań oraz kończyć ustaleniami dotyczącymi rozwoju."),
            ]),
    ];

    private const string NotFoundAnswer =
        "**Nie znalazłem tego w bazie wiedzy.** Nie chcę zgadywać w sprawie zasad oceny, " +
        "dlatego proponuję sprawdzić to w rozporządzeniu albo zapytać komórkę kadrową.";

    public async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var updates = new List<ChatResponseUpdate>();
        await foreach (var update in GetStreamingResponseAsync(messages, options, cancellationToken))
            updates.Add(update);
        return updates.ToChatResponse();
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.Delay(600, cancellationToken);

        var question = Normalize(messages.LastOrDefault(m => m.Role == ChatRole.User)?.Text ?? "");
        var answer = Answers.FirstOrDefault(a => a.Keywords.Any(question.Contains));

        foreach (var token in Tokenize(answer?.Text ?? NotFoundAnswer))
        {
            await Task.Delay(18, cancellationToken);
            yield return new ChatResponseUpdate(ChatRole.Assistant, token);
        }

        if (answer is { Sources.Count: > 0 })
        {
            var content = new TextContent("")
            {
                Annotations = [.. answer.Sources.Select(s => new CitationAnnotation { Title = s.Title, Snippet = s.Snippet })],
            };
            yield return new ChatResponseUpdate(ChatRole.Assistant, [content]);
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;

    public void Dispose()
    {
    }

    private static IEnumerable<string> Tokenize(string text)
    {
        var start = 0;
        for (var i = 1; i <= text.Length; i++)
        {
            if (i == text.Length || (char.IsWhiteSpace(text[i]) && !char.IsWhiteSpace(text[i - 1])))
            {
                yield return text[start..i];
                start = i;
            }
        }
    }

    private static string Normalize(string text)
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

    private sealed record Source(string Title, string Snippet);

    private sealed record CannedAnswer(string[] Keywords, string Text, IReadOnlyList<Source> Sources);
}
