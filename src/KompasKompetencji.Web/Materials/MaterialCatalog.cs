namespace KompasKompetencji.Web.Materials;

public sealed record Material(string FileName, string Title, string Description, string Group)
{
    public string Url => $"{MaterialEndpoints.BasePath}/{FileName}";

    public string Extension => Path.GetExtension(FileName).TrimStart('.').ToUpperInvariant();

    public string Icon => Extension switch
    {
        "PDF" => "picture_as_pdf",
        "XLSX" => "table_chart",
        _ => "description",
    };
}

public static class MaterialCatalog
{
    public const string PeriodicGroup = "Ocena okresowa";
    public const string FirstGroup = "Pierwsza ocena (nowi pracownicy)";

    public static IReadOnlyList<Material> All { get; } =
    [
        new("ustawa-o-sluzbie-cywilnej-2026-poz-590.pdf", "Ustawa o służbie cywilnej",
            "Tekst jednolity. Ocena okresowa: art. 81–83, skutki ocen negatywnych: art. 71.", PeriodicGroup),
        new("rozporzadzenie-oceny-okresowe-2016.pdf", "Rozporządzenie w sprawie ocen okresowych (2016)",
            "Tryb sporządzania oceny, kryteria, skala i wzory arkuszy.", PeriodicGroup),
        new("poradnik-oceny-okresowe.pdf", "Poradnik: oceny okresowe w służbie cywilnej",
            "Praktyczne wyjaśnienia KPRM dla oceniających.", PeriodicGroup),
        new("arkusz-oceny-b.docx", "Arkusz oceny B",
            "Dla osób kierujących zespołem.", PeriodicGroup),
        new("arkusz-oceny-c.docx", "Arkusz oceny C",
            "Dla osób, które nie kierują zespołem.", PeriodicGroup),
        new("model-kompetencji.xlsx", "Model kompetencji",
            "14 kompetencji, 49 wskaźników behawioralnych i opisy 5 poziomów.", PeriodicGroup),
        new("rozporzadzenie-pierwsza-ocena-2015.pdf", "Rozporządzenie w sprawie pierwszej oceny (2015)",
            "Zasady pierwszej oceny w służbie przygotowawczej.", FirstGroup),
        new("poradnik-pierwsza-ocena-2016.pdf", "Poradnik: pierwsza ocena w służbie cywilnej (2016)",
            "Wyjaśnienia KPRM do pierwszej oceny.", FirstGroup),
        new("arkusz-pierwszej-oceny.docx", "Arkusz pierwszej oceny",
            "Wzór arkusza pierwszej oceny.", FirstGroup),
    ];

    public static Material? Find(string fileName) =>
        All.FirstOrDefault(m => string.Equals(m.FileName, fileName, StringComparison.OrdinalIgnoreCase));
}
