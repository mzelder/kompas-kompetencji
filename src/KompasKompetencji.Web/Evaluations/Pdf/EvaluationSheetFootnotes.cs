using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations.Pdf;

public static class EvaluationSheetFootnotes
{
    public const int SheetPurpose = 1;
    public const int Position = 2;
    public const int AdditionalCriteria = 3;
    public const int StartingConversation = 4;
    public const int AssessmentConversation = 5;
    public const int PeriodFrom = 6;
    public const int PeriodTo = 7;
    public const int PartialScore = 8;
    public const int AdditionalCriteriaScores = 9;
    public const int Positive = 10;
    public const int Negative = 11;
    public const int Justification = 12;
    public const int DevelopmentProgram = 13;
    public const int PreviousResult = 14;
    public const int PreviousLevel = 15;
    public const int PromotionRequest = 16;
    public const int Objection = 17;

    public static IReadOnlyList<int> PartI { get; } = [SheetPurpose, Position, AdditionalCriteria, StartingConversation];

    public static IReadOnlyList<int> PartII { get; } =
    [
        AssessmentConversation, PeriodFrom, PeriodTo, PartialScore, AdditionalCriteriaScores, Positive, Negative,
        Justification, DevelopmentProgram, PreviousResult, PreviousLevel, PromotionRequest, Objection,
    ];

    public static int Annex(EvaluationSheet sheet) => sheet == EvaluationSheet.B ? 3 : 4;

    public static string Text(int number, EvaluationSheet sheet) => number switch
    {
        SheetPurpose => sheet == EvaluationSheet.B
            ? "Arkusz służy do sporządzenia oceny okresowej urzędnika służby cywilnej lub pracownika służby cywilnej, zatrudnionego na stanowisku, "
                + "na którym praca polega na bezpośrednim kierowaniu pracownikami."
            : "Arkusz służy do sporządzenia oceny okresowej urzędnika służby cywilnej lub pracownika służby cywilnej, zatrudnionego na stanowisku, "
                + "które nie jest: – stanowiskiem kierownika urzędu, będącym stanowiskiem pracy w służbie cywilnej, ani – stanowiskiem, "
                + "na którym praca polega na bezpośrednim kierowaniu pracownikami.",
        Position => "Należy wskazać pełną nazwę stanowiska pracy wraz z jego rozszerzeniem określającym, do jakich spraw jest to stanowisko "
            + "(jeżeli zostało to określone w opisie stanowiska pracy).",
        AdditionalCriteria or AdditionalCriteriaScores => "Należy wpisać nazwy wybranych kryteriów, korzystając z załącznika nr 1 do rozporządzenia "
            + "Prezesa Rady Ministrów z dnia 4 kwietnia 2016 r. w sprawie warunków i sposobu przeprowadzania ocen okresowych urzędników "
            + "służby cywilnej i pracowników służby cywilnej.",
        StartingConversation => "W trakcie rozmowy omawiane są: 1) główne obowiązki wynikające z opisu stanowiska pracy zajmowanego przez ocenianego, "
            + "2) kryteria obowiązkowe i kryteria dodatkowe, jeżeli są wybierane przez oceniającego, 3) oczekiwania oceniającego co do sposobu "
            + "spełniania kryteriów oceny przez ocenianego, 4) cele do osiągnięcia w okresie, w którym oceniany podlega ocenie, jeżeli ich "
            + "określenie jest uzasadnione ze względu na charakter obowiązków wynikających z opisu stanowiska pracy zajmowanego przez ocenianego, "
            + "a także sposób realizacji wyznaczonych celów.",
        AssessmentConversation => "W trakcie rozmowy oceniającej omawia się: 1) główne obowiązki wykonywane przez ocenianego w okresie, w którym "
            + "podlegał ocenie, oraz sposób ich realizacji, z uwzględnieniem spełniania przez ocenianego ustalonych kryteriów oceny, 2) sposób "
            + "realizowania przez ocenianego celów do osiągnięcia w okresie, w którym oceniany podlegał ocenie, jeżeli cele te zostały wyznaczone, "
            + "3) kierunki dalszego rozwoju zawodowego i potrzeby ocenianego w zakresie podnoszenia kwalifikacji i doskonalenia umiejętności.",
        PeriodFrom => "Data „od” oznacza datę zapoznania ocenianego z kryteriami oceny – z części I arkusza oceny.",
        PeriodTo => "Data „do” oznacza datę sporządzenia oceny na piśmie. Jeżeli ocena okresowa jest sporządzana w związku z zastosowaniem "
            + "art. 81 ust. 6 ustawy z dnia 21 listopada 2008 r. o służbie cywilnej (Dz. U. z 2014 r. poz. 1111, z późn. zm.) i nie została "
            + "sporządzona przed zmianą stanowiska pracy, należy w tym polu wpisać ostatni dzień pracy na zajmowanym stanowisku pracy.",
        PartialScore or PromotionRequest => "Należy wstawić znak „X” w odpowiednie pole.",
        Positive => "POZYTYWNA – w przypadku ustalenia ogólnego poziomu spełniania kryteriów oceny na poziomie „znacznie powyżej oczekiwań”, "
            + "„powyżej oczekiwań” albo „na poziomie oczekiwań”, pod warunkiem nieuzyskania przez ocenianego żadnej z ocen cząstkowych "
            + "na poziomie „znacznie poniżej oczekiwań”.",
        Negative => "NEGATYWNA – w przypadku ustalenia ogólnego poziomu spełniania kryteriów oceny na poziomie „poniżej oczekiwań” albo "
            + "„znacznie poniżej oczekiwań”, a także w przypadku uzyskania przez ocenianego co najmniej jednej oceny cząstkowej na poziomie "
            + "„znacznie poniżej oczekiwań”.",
        Justification => "Uzasadnienie jest obligatoryjne w przypadku otrzymania przez ocenianego negatywnej oceny okresowej. W przypadku "
            + "przyznania oceny pozytywnej uzasadnienie jest fakultatywne. Sporządzenie uzasadnienia oceny może polegać na ogólnym omówieniu "
            + "oceny lub na szczegółowym odniesieniu się do wszystkich ocen cząstkowych albo tylko do niektórych z nich.",
        DevelopmentProgram => "Uwzględniając sporządzoną ocenę, realizowane i przyszłe zadania oraz oczekiwania ocenianego, należy określić "
            + "kierunki rozwoju zawodowego ocenianego, biorąc pod uwagę obszary wiedzy i umiejętności wymagające doskonalenia, oraz "
            + "zaproponować optymalne formy i metody ich realizacji.",
        PreviousResult => "Należy wpisać „pozytywna” lub „negatywna”.",
        PreviousLevel => "Należy wpisać, na jakim poziomie była przyznana poprzednia ocena okresowa (np. „na poziomie oczekiwań”).",
        Objection => "W urzędzie, w którym przepisy nie przewidują tworzenia stanowiska dyrektora generalnego urzędu, sprzeciw składa się "
            + "do kierownika tego urzędu.",
        _ => throw new ArgumentOutOfRangeException(nameof(number)),
    };
}
