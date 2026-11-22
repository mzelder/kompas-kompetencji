using KompasKompetencji.Web.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace KompasKompetencji.Web.Evaluations.Pdf;

public sealed class EvaluationSheetDocument(EvaluationSheetData data) : IDocument
{
    private const float BorderWidth = 0.75f;
    private const string Blank = "";

    private static readonly string[] ScoreLevels =
        ["Znacznie poniżej oczekiwań", "Poniżej oczekiwań", "Na poziomie oczekiwań", "Powyżej oczekiwań", "Znacznie powyżej oczekiwań"];

    private static readonly string[] ScorePoints = ["1 punkt", "3 punkty", "5 punktów", "7 punktów", "9 punktów"];

    private static readonly (string Points, string Level)[] AverageLevels =
    [
        ("od 1 punktu do 2 punktów", "znacznie poniżej oczekiwań"),
        ("powyżej 2 punktów do 4 punktów", "poniżej oczekiwań"),
        ("powyżej 4 punktów do 6 punktów", "na poziomie oczekiwań"),
        ("powyżej 6 punktów do 8 punktów", "powyżej oczekiwań"),
        ("powyżej 8 punktów do 9 punktów", "znacznie powyżej oczekiwań"),
    ];

    public byte[] Generate() => this.GeneratePdf();

    public DocumentMetadata GetMetadata() => new()
    {
        Title = $"Arkusz {data.Sheet} – ocena okresowa – {data.Employee.FirstName} {data.Employee.LastName}",
        Author = $"{data.Evaluator.FirstName} {data.Evaluator.LastName}",
    };

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(1.6f, Unit.Centimetre);
            page.DefaultTextStyle(style => style.FontSize(9));
            page.Content().Column(column =>
            {
                column.Spacing(8);
                ComposeTitle(column);
                ComposePartI(column);
                column.Item().PageBreak();
                ComposePartII(column);
            });
            page.Footer().AlignCenter().Text(text =>
            {
                text.DefaultTextStyle(style => style.FontSize(8).FontColor(Colors.Grey.Darken1));
                text.Span("Strona ");
                text.CurrentPageNumber();
                text.Span(" z ");
                text.TotalPages();
            });
        });
    }

    private void ComposeTitle(ColumnDescriptor column)
    {
        column.Item().AlignCenter().Text($"ARKUSZ {data.Sheet}").Bold().FontSize(14);
        column.Item().AlignCenter().Text(text =>
        {
            text.AlignCenter();
            text.Line("OCENA OKRESOWA").Bold().FontSize(11);
            text.Line("URZĘDNIKA SŁUŻBY CYWILNEJ LUB PRACOWNIKA SŁUŻBY CYWILNEJ").Bold();
        });
    }

    private void ComposePartI(ColumnDescriptor column)
    {
        SectionTitle(column, "CZĘŚĆ I");

        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(1);
                c.RelativeColumn(2);
            });
            HeaderCell(table.Cell()).Text("NAZWA URZĘDU").Bold();
            Cell(table.Cell()).MinHeight(22).Text(Blank);
        });

        ComposePerson(column, "DANE OCENIANEGO", data.Employee);
        ComposePerson(column, "DANE OCENIAJĄCEGO", data.Evaluator);

        Heading(column, "KRYTERIA OCENY I TERMIN SPORZĄDZENIA OCENY");
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(4);
                c.RelativeColumn(2);
                c.RelativeColumn(2);
            });
            HeaderCell(table.Cell()).Text("Kryteria obowiązkowe").Bold();
            HeaderCell(table.Cell().ColumnSpan(2)).Text("Kryteria dodatkowe wybrane przez oceniającego (nie więcej niż 3)").Bold();

            for (var i = 0; i < data.MandatoryCriteria.Count; i++)
            {
                Cell(table.Cell()).Text(data.MandatoryCriteria[i]);
                if (i < EvaluationCriteria.MaxAdditional)
                {
                    Cell(table.Cell().ColumnSpan(2)).Text(i < data.AdditionalCriteria.Count ? data.AdditionalCriteria[i] : Blank);
                }
                else if (i == EvaluationCriteria.MaxAdditional)
                {
                    HeaderCell(table.Cell().RowSpan(2)).AlignMiddle().Text("Termin sporządzenia oceny").Bold();
                    Cell(table.Cell().RowSpan(2)).AlignMiddle().AlignCenter().Text(Date(data.DueDate));
                }
            }
        });

        column.Item().Border(BorderWidth).Padding(6).Column(box =>
        {
            box.Spacing(4);
            box.Item().Text("Potwierdzamy:").Bold();
            box.Item().Text(
                "– przeprowadzenie rozmowy, o której mowa w § 5 ust. 1 rozporządzenia Prezesa Rady Ministrów z dnia 4 kwietnia 2016 r. "
                + "w sprawie warunków i sposobu przeprowadzania ocen okresowych urzędników służby cywilnej i pracowników służby cywilnej "
                + "(Dz. U. poz. 470), i omówienie w trakcie tej rozmowy wszystkich wymaganych zagadnień,");
            box.Item().Text("– zapoznanie ocenianego z kryteriami oceny oraz terminem sporządzenia oceny.");
            box.Item().PaddingTop(10).Element(e => Signatures(e, (Blank, "miejscowość"), (Date(data.PartIConfirmedOn), "data (dd.mm.rrrr)"), (Blank, "podpis oceniającego")));
            box.Item().PaddingTop(6).Element(e => Signatures(e, (Blank, "miejscowość"), (Date(data.EmployeeAcquaintedOn), "data (dd.mm.rrrr)"), (Blank, "podpis ocenianego")));
        });
    }

    private void ComposePartII(ColumnDescriptor column)
    {
        SectionTitle(column, "CZĘŚĆ II");

        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(4);
                c.RelativeColumn(1);
                c.RelativeColumn(2);
                c.RelativeColumn(1);
                c.RelativeColumn(2);
            });
            HeaderCell(table.Cell()).Text("DATA PRZEPROWADZENIA ROZMOWY OCENIAJĄCEJ").Bold();
            Cell(table.Cell().ColumnSpan(4)).AlignCenter().Text(Date(data.AssessmentConversationOn));
            HeaderCell(table.Cell()).Text("OKRES, W KTÓRYM OCENIANY PODLEGAŁ OCENIE").Bold();
            HeaderCell(table.Cell()).AlignMiddle().Text("OD:").Bold();
            Cell(table.Cell()).AlignMiddle().AlignCenter().Text(Date(data.PeriodStart));
            HeaderCell(table.Cell()).AlignMiddle().Text("DO:").Bold();
            Cell(table.Cell()).AlignMiddle().AlignCenter().Text(Date(data.PreparedOn));
        });

        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(3.2f);
                for (var i = 0; i < ScoreLevels.Length; i++)
                {
                    c.RelativeColumn(1);
                }
            });

            ScoreHeader(table, "KRYTERIA OBOWIĄZKOWE", withLevels: true);
            foreach (var criterion in data.MandatoryCriteria)
            {
                ScoreRow(table, criterion);
            }

            ScoreHeader(table, "KRYTERIA DODATKOWE WYBRANE PRZEZ OCENIAJĄCEGO", withLevels: false);
            for (var i = 0; i < EvaluationCriteria.MaxAdditional; i++)
            {
                ScoreRow(table, i < data.AdditionalCriteria.Count ? data.AdditionalCriteria[i] : Blank);
            }

            HeaderCell(table.Cell()).Text("Średnia arytmetyczna z ocen cząstkowych przyznanych za poszczególne kryteria oceny:").Bold();
            Cell(table.Cell().ColumnSpan((uint)ScoreLevels.Length)).Text(Blank);
        });

        Heading(column, "PRZYZNANIE OCENY");
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(2);
                c.RelativeColumn(3);
                c.RelativeColumn(3);
            });
            HeaderCell(table.Cell()).Text("Należy wstawić znak „X” w odpowiednie pole").Bold();
            HeaderCell(table.Cell()).Text("Liczba punktów stanowiąca średnią arytmetyczną ze wszystkich ocen cząstkowych").Bold();
            HeaderCell(table.Cell()).Text("Ogólny poziom spełnienia kryteriów oceny").Bold();
            foreach (var (points, level) in AverageLevels)
            {
                Cell(table.Cell()).Text(Blank);
                Cell(table.Cell()).Text(points);
                Cell(table.Cell()).Text(level);
            }
        });

        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(4);
                c.RelativeColumn(2);
                c.RelativeColumn(1);
                c.RelativeColumn(2);
                c.RelativeColumn(1);
            });
            HeaderCell(table.Cell()).Text("OCENA OKRESOWA W SŁUŻBIE CYWILNEJ").Bold();
            HeaderCell(table.Cell()).Text("POZYTYWNA").Bold();
            Cell(table.Cell()).Text(Blank);
            HeaderCell(table.Cell()).Text("NEGATYWNA").Bold();
            Cell(table.Cell()).Text(Blank);
        });

        Heading(column, "UZASADNIENIE PRZYZNANEJ OCENY OKRESOWEJ");
        column.Item().Border(BorderWidth).MinHeight(90).Text(Blank);

        Heading(column, "WNIOSKI DOTYCZĄCE INDYWIDUALNEGO PROGRAMU ROZWOJU ZAWODOWEGO");
        column.Item().Border(BorderWidth).MinHeight(90).Text(Blank);

        Heading(column, "DANE DOTYCZĄCE POPRZEDNIEJ OCENY OKRESOWEJ URZĘDNIKA SŁUŻBY CYWILNEJ");
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(1);
                c.RelativeColumn(2);
                c.RelativeColumn(1);
                c.RelativeColumn(2);
                c.RelativeColumn(2);
                c.RelativeColumn(2);
            });
            HeaderCell(table.Cell()).Text("Ocena").Bold();
            Cell(table.Cell()).Text(Blank);
            HeaderCell(table.Cell()).Text("Poziom").Bold();
            Cell(table.Cell()).Text(Blank);
            HeaderCell(table.Cell()).Text("Data sporządzenia").Bold();
            Cell(table.Cell()).Text(Blank);
        });

        Heading(column, "CZY DO OCENY OKRESOWEJ DOŁĄCZONO WNIOSEK O PRZYZNANIE KOLEJNEGO STOPNIA SŁUŻBOWEGO?");
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(1);
                c.RelativeColumn(1);
                c.RelativeColumn(1);
                c.RelativeColumn(1);
                c.RelativeColumn(4);
                c.RelativeColumn(1);
            });
            HeaderCell(table.Cell()).Text("TAK").Bold();
            Cell(table.Cell()).Text(Blank);
            HeaderCell(table.Cell()).Text("NIE").Bold();
            Cell(table.Cell()).Text(Blank);
            HeaderCell(table.Cell()).Text("NIE DOTYCZY (oceniany jest pracownikiem służby cywilnej)").Bold();
            Cell(table.Cell()).Text(Blank);
        });

        column.Item().ShowEntire().Column(evaluator =>
        {
            evaluator.Spacing(8);
            ComposePerson(evaluator, "DANE OCENIAJĄCEGO", data.Evaluator);
            evaluator.Item().Element(e => Signatures(e, (Blank, "miejscowość"), (Date(data.PreparedOn), "data (dd.mm.rrrr)"), (Blank, "podpis oceniającego")));
        });

        column.Item().ShowEntire().Border(BorderWidth).Padding(6).Column(box =>
        {
            box.Spacing(4);
            box.Item().Text(
                "Przeprowadzono ze mną rozmowę oceniającą oraz zapoznałem(-łam) się z oceną okresową i wnioskami dotyczącymi "
                + "indywidualnego programu rozwoju zawodowego.");
            box.Item().PaddingTop(10).Element(e => Signatures(e,
                (Blank, "miejscowość"),
                (Date(data.PartIIAcquaintedOn), "data (dd.mm.rrrr)"),
                (Blank, "imię i nazwisko lub pieczątka ocenianego"),
                (Blank, "podpis ocenianego")));
        });

        column.Item().ShowEntire().Border(BorderWidth).Padding(6).Column(box =>
        {
            box.Spacing(4);
            box.Item().Text("Pouczenie").Bold();
            box.Item().Text(
                "Zgodnie z art. 83 ust. 1 ustawy z dnia 21 listopada 2008 r. o służbie cywilnej od oceny okresowej służy, "
                + "w terminie 7 dni od dnia zapoznania się z oceną, sprzeciw do dyrektora generalnego urzędu.");
        });
    }

    private static void ComposePerson(ColumnDescriptor column, string title, SheetPerson person)
    {
        Heading(column, title);
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(1);
                c.RelativeColumn(2);
            });
            foreach (var (label, value) in new[]
            {
                ("Imię", person.FirstName),
                ("Nazwisko", person.LastName),
                ("Komórka organizacyjna", person.Department),
                ("Stanowisko pracy", person.Position),
            })
            {
                HeaderCell(table.Cell()).Text(label).Bold();
                Cell(table.Cell()).Text(value);
            }
        });
    }

    private static void ScoreHeader(TableDescriptor table, string title, bool withLevels)
    {
        var rows = withLevels ? 3u : 1u;
        HeaderCell(table.Cell().RowSpan(rows)).AlignMiddle().Text(title).Bold();
        HeaderCell(table.Cell().ColumnSpan((uint)ScoreLevels.Length)).AlignCenter().Text("OCENA CZĄSTKOWA").Bold();
        if (!withLevels)
        {
            return;
        }

        foreach (var level in ScoreLevels)
        {
            HeaderCell(table.Cell()).AlignCenter().AlignMiddle().Text(level).FontSize(7.5f);
        }
        foreach (var points in ScorePoints)
        {
            HeaderCell(table.Cell()).AlignCenter().Text(points).FontSize(7.5f).Bold();
        }
    }

    private static void ScoreRow(TableDescriptor table, string criterion)
    {
        Cell(table.Cell()).MinHeight(18).Text(criterion);
        for (var i = 0; i < ScoreLevels.Length; i++)
        {
            Cell(table.Cell()).Text(Blank);
        }
    }

    private static void Signatures(IContainer container, params (string Value, string Caption)[] fields)
    {
        container.Row(row =>
        {
            row.Spacing(16);
            foreach (var (value, caption) in fields)
            {
                row.RelativeItem().Column(field =>
                {
                    field.Item().BorderBottom(BorderWidth).BorderColor(Colors.Grey.Darken1).MinHeight(16).AlignBottom().AlignCenter().Text(value);
                    field.Item().AlignCenter().Text(caption).FontSize(7.5f).FontColor(Colors.Grey.Darken2);
                });
            }
        });
    }

    private static void SectionTitle(ColumnDescriptor column, string title) =>
        column.Item().PaddingTop(6).AlignCenter().Text(title).Bold().FontSize(12);

    private static void Heading(ColumnDescriptor column, string title) =>
        column.Item().PaddingTop(4).Text(title).Bold();

    private static IContainer Cell(IContainer container) =>
        container.Border(BorderWidth).PaddingVertical(3).PaddingHorizontal(4);

    private static IContainer HeaderCell(IContainer container) =>
        Cell(container.Background(Colors.Grey.Lighten3));

    private static string Date(DateOnly? date) => date?.ToString("dd.MM.yyyy") ?? Blank;
}
