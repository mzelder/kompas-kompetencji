using System.Text;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Text;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Web.Assistant;

public sealed record NoteClassification(Criterion? Criterion, NoteTone Tone);

public sealed class NoteCriterionClassifier(
    [FromKeyedServices(NoteCriterionClassifier.ClientKey)] IChatClient chatClient,
    ILogger<NoteCriterionClassifier> logger)
{
    public const string ClientKey = "note-criteria";

    private const string NoCriterion = "BRAK";
    private const int StemLength = 6;

    private const string Instructions =
        "Jesteś asystentem przełożonego w służbie cywilnej. Przełożony zapisuje notatki o pracy podwładnego " +
        "w trakcie okresu oceny. Dla każdej notatki ustal dwie rzeczy:\n" +
        "1. Kryterium oceny, którego notatka dotyczy najbardziej: kod jednego kryterium z listy. " +
        "Kryterium nie musi pasować idealnie. Wystarczy, że notatka choć trochę go dotyczy: przejdź po kolei " +
        "przez wszystkie kryteria z listy, odrzuć te, które zupełnie nie pasują, i z pozostałych wybierz najbliższe. " +
        $"Odpowiedz {NoCriterion} tylko wtedy, gdy notatka nie ma żadnego związku z żadnym kryterium z listy.\n" +
        "2. Wydźwięk opisanej sytuacji: Positive (pracownik zrobił coś dobrze), Neutral (sam fakt, bez wyraźnej oceny " +
        "albo dobre i złe strony po równo) albo Negative (coś poszło źle, uchybienie, problem).\n" +
        "Nie oceniasz pracownika i nie proponujesz poziomu oceny. " +
        "Odpowiedz dokładnie w dwóch liniach, bez żadnego innego tekstu:\n" +
        "KRYTERIUM: <kod>\nWYDZWIEK: <Positive|Neutral|Negative>";

    private static readonly IReadOnlyDictionary<Criterion, string> Descriptions = new Dictionary<Criterion, string>
    {
        [Criterion.Communication] = "jasne przekazywanie informacji ustnie i na piśmie, odpowiedzi na pisma i pytania",
        [Criterion.Creativity] = "nowe pomysły, usprawnienia, niestandardowe rozwiązania",
        [Criterion.Negotiation] = "uzgadnianie stanowisk, kompromisy, porozumienia",
        [Criterion.WorkOrganization] = "planowanie, terminowość, priorytety, realizacja celów",
        [Criterion.CustomerOrientation] = "obsługa interesantów i odbiorców zewnętrznych, odpowiadanie na ich potrzeby",
        [Criterion.DecisionMaking] = "podejmowanie decyzji, branie odpowiedzialności za ich skutki",
        [Criterion.CrisisManagement] = "działanie pod presją, w sytuacjach nagłych, awariach i kryzysach",
        [Criterion.Reliability] = "dokładność, poprawność, sprawdzanie, wykrywanie i unikanie błędów",
        [Criterion.AnalyticalThinking] = "analiza danych i informacji, wyciąganie wniosków, prognozy, wskaźniki",
        [Criterion.KnowledgeAndDevelopment] = "wykorzystywanie wiedzy fachowej, dzielenie się nią, szkolenia i rozwój",
        [Criterion.Cooperation] = "praca z innymi osobami i komórkami, pomoc kolegom, wspólne zadania",
        [Criterion.PublicSpeaking] = "wystąpienia, prezentacje, konferencje",
        [Criterion.PeopleManagement] = "kierowanie zespołem, delegowanie, motywowanie i rozwój podwładnych",
        [Criterion.StrategicManagement] = "myślenie długofalowe, cele strategiczne, kierunki działania urzędu",
    };

    private static readonly IReadOnlyDictionary<Criterion, string[]> Keywords = new Dictionary<Criterion, string[]>
    {
        [Criterion.Communication] = ["komunik", "pism", "odpowiedz", "odpowiedź", "wyjaśni", "informow", "korespond"],
        [Criterion.Creativity] = ["pomysł", "usprawni", "innowac", "zaproponow", "nowe rozwiąz"],
        [Criterion.Negotiation] = ["negocj", "porozumi", "kompromis"],
        [Criterion.WorkOrganization] = ["termin", "plan", "priorytet", "harmonogram", "organiz", "cel"],
        [Criterion.CustomerOrientation] = ["interesant", "klient", "petent", "obywatel", "wniosk"],
        [Criterion.DecisionMaking] = ["decyz", "zdecydow", "odpowiedzialn"],
        [Criterion.CrisisManagement] = ["kryzys", "awari", "presj", "nagł", "pilne"],
        [Criterion.Reliability] = ["błąd", "błęd", "poprawk", "dokładn", "rzeteln", "sprawdz", "weryfik", "kontrol"],
        [Criterion.AnalyticalThinking] = ["analiz", "dane", "danych", "wnios", "wskaźnik", "prognoz", "statyst"],
        [Criterion.KnowledgeAndDevelopment] = ["szkoleni", "wiedz", "warsztat", "kurs", "doskonal", "nauczy", "przepis"],
        [Criterion.Cooperation] = ["współprac", "zespoł", "koleg", "uzgodni", "pomóg", "pomoc"],
        [Criterion.PublicSpeaking] = ["wystąpi", "prezentac", "konferencj", "prelekcj"],
        [Criterion.PeopleManagement] = ["podwładn", "delegow", "motywow", "zespołem"],
        [Criterion.StrategicManagement] = ["strateg", "długofal", "długotermin"],
    };

    private static readonly string[] PositiveWords =
        ["sprawnie", "przed termin", "bez poprawek", "samodzieln", "z własnej inicjatywy", "dobrze", "świetn", "bardzo dobr",
         "pochwał", "docenion", "przydatn", "zaproponował", "wykrył", "usprawnił", "uporządkował", "skuteczn", "terminowo", "w terminie"];

    private static readonly string[] NegativeWords =
        ["nie oddał", "nie oddała", "nie dotrzym", "po terminie", "spóźni", "opóźni", "pomyłk", "zaniedb", "skarg", "uchybieni",
         "nie przygotow", "nie wykonał", "nie zgłosił", "zignorow", "konflikt", "niestaranni", "niedokładn", "nieterminow"];

    private static readonly HashSet<string> IgnoredStems = ["przez", "oraz", "ktory", "ktora", "ktore", "sobie", "tego", "jego", "jest", "bylo", "byla", "takze", "innych"];

    public async Task<NoteClassification> ClassifyAsync(string note, IReadOnlyList<Criterion> criteria, CancellationToken ct = default)
    {
        if (chatClient.GetService<MockChatClient>() is null)
        {
            try
            {
                var response = await chatClient.GetResponseAsync(
                    [new ChatMessage(ChatRole.User, BuildPrompt(note, criteria))],
                    new ChatOptions { Instructions = Instructions },
                    ct);
                if (ParseAnswer(response.Text, criteria) is NoteClassification classification)
                {
                    return classification;
                }
                logger.LogWarning("Model zwrócił odpowiedź w nieznanym formacie: {Answer}", response.Text);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogWarning(ex, "Nie udało się sklasyfikować notatki przez model.");
            }
        }

        return new NoteClassification(MatchByKeywords(note, criteria), ToneByKeywords(note));
    }

    public static NoteClassification? ParseAnswer(string? answer, IReadOnlyList<Criterion> criteria)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            return null;
        }

        var lines = answer.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var criterionLine = lines.FirstOrDefault(l => l.StartsWith("KRYTERIUM", StringComparison.OrdinalIgnoreCase));
        var toneLine = lines.FirstOrDefault(l => l.StartsWith("WYDZWIEK", StringComparison.OrdinalIgnoreCase)
                                                 || l.StartsWith("WYDŹWIĘK", StringComparison.OrdinalIgnoreCase));
        if (criterionLine is null || toneLine is null || ParseTone(toneLine) is not NoteTone tone)
        {
            return null;
        }

        var criterion = FirstMentioned(criterionLine, criteria);
        if (criterion is null && !criterionLine.Contains(NoCriterion, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        return new NoteClassification(criterion, tone);
    }

    public static Criterion? MatchByKeywords(string note, IReadOnlyList<Criterion> criteria)
    {
        var text = TextNormalizer.Normalize(note);
        var noteStems = Stems(text);
        var best = criteria
            .Select((c, order) => (Criterion: c, Order: order, Score: KeywordScore(text, c) * 2 + DescriptionScore(noteStems, c)))
            .OrderByDescending(m => m.Score)
            .ThenBy(m => m.Order)
            .FirstOrDefault();
        return best.Score > 0 ? best.Criterion : null;
    }

    public static NoteTone ToneByKeywords(string note)
    {
        var text = TextNormalizer.Normalize(note);
        var positive = PositiveWords.Sum(w => CountOccurrences(text, TextNormalizer.Normalize(w)));
        var negative = NegativeWords.Sum(w => CountOccurrences(text, TextNormalizer.Normalize(w)));
        return positive > negative ? NoteTone.Positive
            : negative > positive ? NoteTone.Negative
            : NoteTone.Neutral;
    }

    private static NoteTone? ParseTone(string line) =>
        Enum.GetValues<NoteTone>()
            .Select(t => (Tone: t, Index: line.IndexOf(t.ToString(), StringComparison.OrdinalIgnoreCase)))
            .Where(m => m.Index >= 0)
            .OrderBy(m => m.Index)
            .Select(m => (NoteTone?)m.Tone)
            .FirstOrDefault();

    private static Criterion? FirstMentioned(string line, IReadOnlyList<Criterion> criteria) =>
        criteria
            .Select(c => (Criterion: c, Index: line.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase)))
            .Where(m => m.Index >= 0)
            .OrderBy(m => m.Index)
            .Select(m => (Criterion?)m.Criterion)
            .FirstOrDefault();

    private static string BuildPrompt(string note, IReadOnlyList<Criterion> criteria)
    {
        var prompt = new StringBuilder("Kryteria oceny (kod: nazwa, czego dotyczy):\n");
        foreach (var criterion in criteria)
        {
            prompt.Append($"- {criterion}: {EvaluationCriteria.Name(criterion)}");
            if (Descriptions.TryGetValue(criterion, out var description))
            {
                prompt.Append($" ({description})");
            }
            prompt.Append('\n');
        }
        prompt.Append("\nNotatka przełożonego:\n<notatka>\n").Append(note).Append("\n</notatka>");
        return prompt.ToString();
    }

    private static int KeywordScore(string normalizedNote, Criterion criterion) =>
        Keywords.TryGetValue(criterion, out var words)
            ? words.Sum(w => CountOccurrences(normalizedNote, TextNormalizer.Normalize(w)))
            : 0;

    private static int DescriptionScore(HashSet<string> noteStems, Criterion criterion)
    {
        var description = EvaluationCriteria.Name(criterion) + " " + Descriptions.GetValueOrDefault(criterion, string.Empty);
        return Stems(TextNormalizer.Normalize(description)).Count(noteStems.Contains);
    }

    private static HashSet<string> Stems(string normalizedText) =>
        normalizedText
            .Split(normalizedText.Where(c => !char.IsLetter(c)).Distinct().ToArray(), StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length >= 4 && !IgnoredStems.Contains(w))
            .Select(w => w.Length > StemLength ? w[..StemLength] : w)
            .ToHashSet();

    private static int CountOccurrences(string text, string word)
    {
        var count = 0;
        for (var index = text.IndexOf(word, StringComparison.Ordinal); index >= 0; index = text.IndexOf(word, index + word.Length, StringComparison.Ordinal))
        {
            count++;
        }
        return count;
    }
}
