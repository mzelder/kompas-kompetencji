using System.Globalization;
using KompasKompetencji.Web.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static KompasKompetencji.Web.Evaluations.Pdf.EvaluationSheetFootnotes;

namespace KompasKompetencji.Web.Evaluations.Pdf;

public sealed class EvaluationSheetDocument(EvaluationSheetData data) : IDocument
{
    private const float BorderWidth = 0.75f;
    private const float SmallText = 7.5f;
    private const float FootnoteText = 6.5f;
    private const string Blank = "";
    private const string Mark = "X";

    private static readonly CultureInfo Polish = CultureInfo.GetCultureInfo("pl-PL");

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
                column.Spacing(6);
                ComposeTitle(column);
                ComposePartI(column);
                Footnotes(column, PartI);
                column.Item().PageBreak();
                ComposePartII(column);
                Footnotes(column, PartII);
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
        column.Item().AlignRight().Text($"Załącznik nr {Annex(data.Sheet)}").FontSize(8).FontColor(Colors.Grey.Darken2);
        column.Item().AlignCenter().Text($"ARKUSZ {data.Sheet}").Bold().FontSize(14);
        column.Item().AlignCenter().Text(text =>
        {
            text.AlignCenter();
            text.Line("OCENA OKRESOWA").Bold().FontSize(11);
            text.Span("URZĘDNIKA SŁUŻBY CYWILNEJ LUB PRACOWNIKA SŁUŻBY CYWILNEJ").Bold();
            Note(text, SheetPurpose);
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
            Label(HeaderCell(table.Cell()), "NAZWA URZĘDU");
            Cell(table.Cell()).MinHeight(22).Text(Blank);
        });

        ComposePerson(column, "DANE OCENIANEGO", data.Employee, Position);
        ComposePerson(column, "DANE OCENIAJĄCEGO", data.Evaluator, null);

        Heading(column, "KRYTERIA OCENY I TERMIN SPORZĄDZENIA OCENY");
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(4);
                c.RelativeColumn(2);
                c.RelativeColumn(2);
            });
            Label(HeaderCell(table.Cell()), "Kryteria obowiązkowe");
            Label(HeaderCell(table.Cell().ColumnSpan(2)), "Kryteria dodatkowe wybrane przez oceniającego (nie więcej niż 3)", AdditionalCriteria);

            for (var i = 0; i < data.MandatoryCriteria.Count; i++)
            {
                Cell(table.Cell()).Text(data.MandatoryCriteria[i].Name);
                if (i < EvaluationCriteria.MaxAdditional)
                {
                    Cell(table.Cell().ColumnSpan(2)).Text(i < data.AdditionalCriteria.Count ? data.AdditionalCriteria[i].Name : Blank);
                }
                else if (i == EvaluationCriteria.MaxAdditional)
                {
                    Label(HeaderCell(table.Cell().RowSpan(2)).AlignMiddle(), "Termin sporządzenia oceny");
                    Cell(table.Cell().RowSpan(2)).AlignMiddle().AlignCenter().Text(Date(data.DueDate));
                }
            }
        });

        column.Item().Border(BorderWidth).Padding(6).Column(box =>
        {
            box.Spacing(4);
            box.Item().Text("Potwierdzamy:").Bold();
            box.Item().Text(text =>
            {
                text.Span(
                    "– przeprowadzenie rozmowy, o której mowa w § 5 ust. 1 rozporządzenia Prezesa Rady Ministrów z dnia 4 kwietnia 2016 r. "
                    + "w sprawie warunków i sposobu przeprowadzania ocen okresowych urzędników służby cywilnej i pracowników służby cywilnej "
                    + "(Dz. U. poz. 470), i omówienie w trakcie tej rozmowy wszystkich wymaganych zagadnień");
                Note(text, StartingConversation);
                text.Span(",");
            });
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
            Label(HeaderCell(table.Cell()), "DATA PRZEPROWADZENIA ROZMOWY OCENIAJĄCEJ", AssessmentConversation);
            Cell(table.Cell().ColumnSpan(4)).AlignMiddle().AlignCenter().Text(Date(data.AssessmentConversationOn));
            Label(HeaderCell(table.Cell()), "OKRES, W KTÓRYM OCENIANY PODLEGAŁ OCENIE");
            Label(HeaderCell(table.Cell()).AlignMiddle(), "OD", PeriodFrom, ":");
            Cell(table.Cell()).AlignMiddle().AlignCenter().Text(Date(data.PeriodStart));
            Label(HeaderCell(table.Cell()).AlignMiddle(), "DO", PeriodTo, ":");
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

            ScoreHeader(table, "KRYTERIA OBOWIĄZKOWE", null, withLevels: true);
            foreach (var criterion in data.MandatoryCriteria)
            {
                ScoreRow(table, criterion);
            }

            ScoreHeader(table, "KRYTERIA DODATKOWE WYBRANE PRZEZ OCENIAJĄCEGO", AdditionalCriteriaScores, withLevels: false);
            for (var i = 0; i < EvaluationCriteria.MaxAdditional; i++)
            {
                ScoreRow(table, i < data.AdditionalCriteria.Count ? data.AdditionalCriteria[i] : null);
            }

            Label(HeaderCell(table.Cell().ColumnSpan(4)).AlignMiddle(), "Średnia arytmetyczna z ocen cząstkowych przyznanych za poszczególne kryteria oceny:");
            Cell(table.Cell().ColumnSpan(2)).AlignMiddle().AlignCenter()
                .Text(data.Summary is { } summary ? $"{summary.AveragePoints.ToString("0.00", Polish)} pkt" : Blank).Bold();
        });

        Section(column, "PRZYZNANIE OCENY", null, body => body.Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(2);
                c.RelativeColumn(3);
                c.RelativeColumn(3);
            });
            Label(HeaderCell(table.Cell()), "Należy wstawić znak „X” w odpowiednie pole");
            Label(HeaderCell(table.Cell()), "Liczba punktów stanowiąca średnią arytmetyczną ze wszystkich ocen cząstkowych");
            Label(HeaderCell(table.Cell()), "Ogólny poziom spełnienia kryteriów oceny");
            foreach (var (points, level) in AverageLevels)
            {
                MarkCell(table.Cell(), string.Equals(data.Summary?.OverallLevel, level, StringComparison.OrdinalIgnoreCase));
                Cell(table.Cell()).Text(points);
                Cell(table.Cell()).Text(level);
            }
        }));

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
            Label(HeaderCell(table.Cell()), "OCENA OKRESOWA W SŁUŻBIE CYWILNEJ");
            Label(HeaderCell(table.Cell()), "POZYTYWNA", Positive);
            MarkCell(table.Cell(), data.Summary?.IsPositive == true);
            Label(HeaderCell(table.Cell()), "NEGATYWNA", Negative);
            MarkCell(table.Cell(), data.Summary?.IsPositive == false);
        });

        Section(column, "UZASADNIENIE PRZYZNANEJ OCENY OKRESOWEJ", Justification, body => body.Border(BorderWidth).MinHeight(90).Text(Blank));

        Section(column, "WNIOSKI DOTYCZĄCE INDYWIDUALNEGO PROGRAMU ROZWOJU ZAWODOWEGO", DevelopmentProgram, body => body.Border(BorderWidth).MinHeight(90).Text(Blank));

        Section(column, "DANE DOTYCZĄCE POPRZEDNIEJ OCENY OKRESOWEJ URZĘDNIKA SŁUŻBY CYWILNEJ", null, body => body.Table(table =>
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
            Label(HeaderCell(table.Cell()), "Ocena", PreviousResult);
            Cell(table.Cell()).Text(data.Previous is { } result ? (result.IsPositive ? "pozytywna" : "negatywna") : Blank);
            Label(HeaderCell(table.Cell()), "Poziom", PreviousLevel);
            Cell(table.Cell()).Text(data.Previous?.OverallLevel.ToLower(Polish) ?? Blank);
            Label(HeaderCell(table.Cell()), "Data sporządzenia");
            Cell(table.Cell()).Text(Date(data.Previous?.PreparedOn));
        }));

        Section(column, "CZY DO OCENY OKRESOWEJ DOŁĄCZONO WNIOSEK O PRZYZNANIE KOLEJNEGO STOPNIA SŁUŻBOWEGO?", PromotionRequest, body => body.Table(table =>
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
            Label(HeaderCell(table.Cell()), "TAK");
            Cell(table.Cell()).Text(Blank);
            Label(HeaderCell(table.Cell()), "NIE");
            Cell(table.Cell()).Text(Blank);
            Label(HeaderCell(table.Cell()), "NIE DOTYCZY (oceniany jest pracownikiem służby cywilnej)");
            Cell(table.Cell()).Text(Blank);
        }));

        column.Item().ShowEntire().Column(evaluator =>
        {
            evaluator.Spacing(8);
            ComposePerson(evaluator, "DANE OCENIAJĄCEGO", data.Evaluator, null);
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
            box.Item().Text(text =>
            {
                text.Span(
                    "Zgodnie z art. 83 ust. 1 ustawy z dnia 21 listopada 2008 r. o służbie cywilnej od oceny okresowej służy, "
                    + "w terminie 7 dni od dnia zapoznania się z oceną, sprzeciw do dyrektora generalnego urzędu");
                Note(text, Objection);
                text.Span(".");
            });
        });
    }

    private void Footnotes(ColumnDescriptor column, IReadOnlyList<int> numbers) =>
        column.Item().ExtendVertical().AlignBottom().ShowEntire().PaddingTop(6).PaddingBottom(4).Column(notes =>
        {
            notes.Spacing(1);
            notes.Item().Width(5, Unit.Centimetre).LineHorizontal(BorderWidth).LineColor(Colors.Grey.Darken1);
            foreach (var number in numbers)
            {
                notes.Item().Text(text =>
                {
                    text.DefaultTextStyle(style => style.FontSize(FootnoteText).FontColor(Colors.Grey.Darken3));
                    text.Span($"{number}) ");
                    text.Span(EvaluationSheetFootnotes.Text(number, data.Sheet));
                });
            }
        });

    private static void ComposePerson(ColumnDescriptor column, string title, SheetPerson person, int? positionNote)
    {
        Heading(column, title);
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                c.RelativeColumn(1);
                c.RelativeColumn(2);
            });
            foreach (var (label, value, note) in new (string, string, int?)[]
            {
                ("Imię", person.FirstName, null),
                ("Nazwisko", person.LastName, null),
                ("Komórka organizacyjna", person.Department, null),
                ("Stanowisko pracy", person.Position, positionNote),
            })
            {
                Label(HeaderCell(table.Cell()), label, note);
                Cell(table.Cell()).Text(value);
            }
        });
    }

    private static void ScoreHeader(TableDescriptor table, string title, int? titleNote, bool withLevels)
    {
        var rows = withLevels ? 3u : 1u;
        Label(HeaderCell(table.Cell().RowSpan(rows)).AlignMiddle(), title, titleNote);
        Label(HeaderCell(table.Cell().ColumnSpan((uint)ScoreLevels.Length)).AlignCenter(), "OCENA CZĄSTKOWA", PartialScore);
        if (!withLevels)
        {
            return;
        }

        foreach (var level in ScoreLevels)
        {
            HeaderCell(table.Cell()).AlignCenter().AlignMiddle().Text(level).FontSize(SmallText);
        }
        foreach (var points in ScorePoints)
        {
            HeaderCell(table.Cell()).AlignCenter().Text(points).FontSize(SmallText).Bold();
        }
    }

    private static void ScoreRow(TableDescriptor table, SheetCriterion? criterion)
    {
        Cell(table.Cell()).MinHeight(18).AlignMiddle().Text(criterion?.Name ?? Blank);
        foreach (var level in CompetencyLevels.All)
        {
            MarkCell(table.Cell(), criterion?.Level == level);
        }
    }

    private static void MarkCell(IContainer container, bool marked) =>
        Cell(container).AlignMiddle().AlignCenter().Text(marked ? Mark : Blank).Bold();

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
                    field.Item().AlignCenter().Text(caption).FontSize(SmallText).FontColor(Colors.Grey.Darken2);
                });
            }
        });
    }

    private static void SectionTitle(ColumnDescriptor column, string title) =>
        column.Item().PaddingTop(6).AlignCenter().Text(title).Bold().FontSize(12);

    private static void Heading(ColumnDescriptor column, string title, int? note = null) =>
        Label(column.Item().PaddingTop(4), title, note);

    private static void Section(ColumnDescriptor column, string title, int? note, Action<IContainer> body) =>
        column.Item().ShowEntire().Column(section =>
        {
            section.Spacing(8);
            Heading(section, title, note);
            body(section.Item());
        });

    private static void Label(IContainer container, string label, int? note = null, string suffix = Blank) =>
        container.Text(text =>
        {
            text.Span(label).Bold();
            if (note is int number)
            {
                Note(text, number);
            }
            text.Span(suffix).Bold();
        });

    private static void Note(TextDescriptor text, int number) => text.Span($"{number})").Superscript();

    private static IContainer Cell(IContainer container) =>
        container.Border(BorderWidth).PaddingVertical(3).PaddingHorizontal(4);

    private static IContainer HeaderCell(IContainer container) =>
        Cell(container.Background(Colors.Grey.Lighten3));

    private static string Date(DateOnly? date) => date?.ToString("dd.MM.yyyy") ?? Blank;
}
