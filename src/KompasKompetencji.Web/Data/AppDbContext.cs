using KompasKompetencji.Web.Evaluations;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<EmployeeSupervisor> EmployeeSupervisors => Set<EmployeeSupervisor>();

    public DbSet<Evaluation> Evaluations => Set<Evaluation>();

    public DbSet<Conversation> Conversations => Set<Conversation>();

    public DbSet<EvaluationAdditionalCriterion> EvaluationAdditionalCriteria => Set<EvaluationAdditionalCriterion>();

    public DbSet<JobPosition> JobPositions => Set<JobPosition>();

    public DbSet<EvaluationNote> EvaluationNotes => Set<EvaluationNote>();

    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<CompetencyIndicator> CompetencyIndicators => Set<CompetencyIndicator>();

    public DbSet<IndicatorBehavior> IndicatorBehaviors => Set<IndicatorBehavior>();

    public DbSet<IndicatorRating> IndicatorRatings => Set<IndicatorRating>();

    public DbSet<CriterionDecision> CriterionDecisions => Set<CriterionDecision>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(u =>
        {
            u.ToTable("Users");
            u.Property(p => p.Login).HasMaxLength(50);
            u.HasIndex(p => p.Login).IsUnique();
            u.Property(p => p.PasswordHash).HasMaxLength(200);
            u.Property(p => p.Role).HasMaxLength(20);
            u.HasIndex(p => p.EmployeeId).IsUnique();
            u.HasOne(p => p.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Employee>(e =>
        {
            e.ToTable("Employees");
            e.Property(p => p.FirstName).HasMaxLength(100);
            e.Property(p => p.LastName).HasMaxLength(100);
            e.Property(p => p.Position).HasMaxLength(200);
            e.Property(p => p.Department).HasMaxLength(200);
            e.HasOne(p => p.JobPosition)
                .WithMany(p => p.Employees)
                .HasForeignKey(p => p.JobPositionId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasData(
                new Employee { Id = 2, FirstName = "Jan", LastName = "Nowak", Position = "Specjalista", Department = "Wydział Analiz Danych", JobPositionId = 2, EmployedSince = new DateOnly(2025, 1, 11) },
                new Employee { Id = 3, FirstName = "Ewa", LastName = "Wiśniewska", Position = "Naczelnik wydziału", Department = "Wydział Analiz Danych", IsSupervisor = true, JobPositionId = 3, EmployedSince = new DateOnly(2015, 4, 1) },
                new Employee { Id = 4, FirstName = "Tomasz", LastName = "Zieliński", Position = "Dyrektor departamentu", Department = "Departament Danych i Usług Cyfrowych", IsSupervisor = true, EmployedSince = new DateOnly(2012, 9, 1) },
                new Employee { Id = 10, FirstName = "Agnieszka", LastName = "Woźniak", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", JobPositionId = 5, EmployedSince = new DateOnly(2020, 6, 1) },
                new Employee { Id = 11, FirstName = "Paweł", LastName = "Kozłowski", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", JobPositionId = 6, EmployedSince = new DateOnly(2026, 11, 5) },
                new Employee { Id = 12, FirstName = "Karolina", LastName = "Mazur", Position = "Specjalista", Department = "Wydział Analiz Danych", JobPositionId = 7, EmployedSince = new DateOnly(2024, 11, 15) },
                new Employee { Id = 14, FirstName = "Natalia", LastName = "Krawczyk", Position = "Główny specjalista", Department = "Wydział Analiz Danych", JobPositionId = 8, EmployedSince = new DateOnly(2025, 1, 20) },
                new Employee { Id = 17, FirstName = "Joanna", LastName = "Pawlak", Position = "Główny specjalista", Department = "Wydział Analiz Danych", JobPositionId = 11, EmployedSince = new DateOnly(2020, 12, 7) });
        });

        modelBuilder.Entity<EmployeeSupervisor>(l =>
        {
            l.ToTable("EmployeeSupervisors", t =>
                t.HasCheckConstraint("CK_EmployeeSupervisors_NotSelf", "[EmployeeId] <> [SupervisorId]"));
            l.HasKey(p => new { p.EmployeeId, p.SupervisorId });
            l.HasOne(p => p.Employee)
                .WithMany(p => p.SupervisorLinks)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            l.HasOne(p => p.Supervisor)
                .WithMany(p => p.SubordinateLinks)
                .HasForeignKey(p => p.SupervisorId)
                .OnDelete(DeleteBehavior.NoAction);

            l.HasData(
                new EmployeeSupervisor { EmployeeId = 2, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 3, SupervisorId = 4 },
                new EmployeeSupervisor { EmployeeId = 10, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 11, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 12, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 14, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 17, SupervisorId = 3 });
        });

        modelBuilder.Entity<JobPosition>(j =>
        {
            j.ToTable("JobPositions");
            j.Property(p => p.Symbol).HasMaxLength(50);
            j.HasIndex(p => p.Symbol).IsUnique();
            j.Property(p => p.Title).HasMaxLength(200);
            j.Property(p => p.Department).HasMaxLength(200);
            j.Property(p => p.Unit).HasMaxLength(200);
            j.Property(p => p.DirectSupervisorTitle).HasMaxLength(200);
            j.Property(p => p.IndirectSupervisorTitle).HasMaxLength(200);
            j.Property(p => p.RequiredCompetencies).HasMaxLength(1000);
            j.Property(p => p.Subordinates).HasConversion<string>().HasMaxLength(20);
            j.HasData(DemoJobPositions.All);
        });

        modelBuilder.Entity<Evaluation>(v =>
        {
            v.ToTable("Evaluations");
            v.Property(p => p.Status).HasConversion<string>().HasMaxLength(30);
            v.Property(p => p.Sheet).HasConversion<string>().HasMaxLength(1);
            v.Property(p => p.Expectations).HasMaxLength(4000);
            v.Property(p => p.Goals).HasMaxLength(4000);
            v.HasIndex(p => new { p.EmployeeId, p.DueDate });
            v.HasOne(p => p.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            v.HasOne(p => p.Evaluator)
                .WithMany()
                .HasForeignKey(p => p.EvaluatorId)
                .OnDelete(DeleteBehavior.NoAction);

            v.HasData(
                new Evaluation
                {
                    Id = 5, EmployeeId = 10, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2024, 11, 12, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 11, 17), PeriodEnd = new DateOnly(2026, 11, 6), DueDate = new DateOnly(2026, 11, 10),
                    Expectations = "Opinie do projektów dokumentów przygotowywane w terminie, z konkretnymi propozycjami zmian wskaźników.",
                    PartIConfirmedAt = new DateTime(2024, 11, 17, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2024, 11, 17),
                    StartedAt = new DateTime(2026, 10, 12, 9, 0, 0), CompletedAt = new DateTime(2026, 11, 6, 13, 0, 0),
                    PreparedOn = new DateOnly(2026, 11, 6), PartIIAcquaintedOn = new DateOnly(2026, 11, 6),
                },
                new Evaluation
                {
                    Id = 6, EmployeeId = 12, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2024, 11, 20, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 12, 2), DueDate = new DateOnly(2026, 11, 15),
                    Expectations = "Cykliczne raporty przekazywane w terminie, z krótkim omówieniem najważniejszych zmian w danych.",
                    PartIConfirmedAt = new DateTime(2024, 12, 2, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2024, 12, 2),
                },
                new Evaluation
                {
                    Id = 9, EmployeeId = 10, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.NotStarted, SheetConfirmedAt = new DateTime(2026, 11, 12, 9, 0, 0),
                    PeriodStart = new DateOnly(2026, 11, 6), DueDate = new DateOnly(2028, 11, 6),
                    Expectations = "Analizy wpływu zmian na usługi cyfrowe z rekomendacją i oceną ryzyka. Samodzielne przedstawianie stanowiska wydziału na posiedzeniach zespołów międzyresortowych.",
                },
                new Evaluation
                {
                    Id = 11, EmployeeId = 14, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2025, 1, 27, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 2, 10), DueDate = new DateOnly(2027, 1, 20),
                    Expectations = "Analizy porównawcze dla kierownictwa przekazywane w uzgodnionych terminach. Koordynowanie prac analitycznych kilku osób wydziału bez opóźnień.",
                    Goals = "Opracowanie standardu opisu i weryfikacji źródeł danych dla wydziału do końca 2026 r.",
                    PartIConfirmedAt = new DateTime(2025, 2, 10, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 2, 10),
                },
                new Evaluation
                {
                    Id = 16, EmployeeId = 17, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2022, 12, 10, 9, 0, 0),
                    PeriodStart = new DateOnly(2022, 12, 20), PeriodEnd = new DateOnly(2024, 12, 5), DueDate = new DateOnly(2024, 12, 7),
                    Expectations = "Raporty kwartalne przekazywane w terminie, z danymi sprawdzonymi z rejestrami źródłowymi.",
                    PartIConfirmedAt = new DateTime(2022, 12, 20, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2022, 12, 20),
                    StartedAt = new DateTime(2024, 10, 14, 9, 0, 0), CompletedAt = new DateTime(2024, 12, 5, 12, 0, 0),
                    PreparedOn = new DateOnly(2024, 12, 5), PartIIAcquaintedOn = new DateOnly(2024, 12, 5),
                },
                new Evaluation
                {
                    Id = 17, EmployeeId = 17, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.InProgress, SheetConfirmedAt = new DateTime(2024, 12, 10, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 12, 18), DueDate = new DateOnly(2026, 12, 7),
                    Expectations = "Dane w raportach sprawdzone z rejestrami przed wysłaniem. Notatki dla kierownictwa z wyraźnie oddzielonymi ustaleniami i kwestiami otwartymi.",
                    PartIConfirmedAt = new DateTime(2024, 12, 18, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2024, 12, 18),
                    StartedAt = new DateTime(2026, 10, 12, 9, 0, 0),
                });
        });

        modelBuilder.Entity<EvaluationNote>(n =>
        {
            n.ToTable("EvaluationNotes");
            n.Property(p => p.Text).HasMaxLength(EvaluationNote.MaxTextLength);
            n.Property(p => p.Criterion).HasConversion<string>().HasMaxLength(40);
            n.Property(p => p.Tone).HasConversion<string>().HasMaxLength(20);
            n.HasIndex(p => new { p.EvaluationId, p.CreatedAt });
            n.HasOne(p => p.Evaluation)
                .WithMany()
                .HasForeignKey(p => p.EvaluationId)
                .OnDelete(DeleteBehavior.Cascade);

            n.HasData(
                new EvaluationNote
                {
                    Id = 1, EvaluationId = 11, Criterion = Criterion.KnowledgeAndDevelopment, Tone = NoteTone.Positive, CreatedAt = new DateTime(2026, 9, 8, 11, 0, 0),
                    Text = "Z własnej inicjatywy poprowadziła dla wydziału warsztat z weryfikacji źródeł danych. Uczestnicy ocenili go jako przydatny.",
                },
                new EvaluationNote
                {
                    Id = 2, EvaluationId = 11, Criterion = Criterion.Reliability, Tone = NoteTone.Negative, CreatedAt = new DateTime(2026, 10, 14, 13, 20, 0),
                    Text = "Przekazała kierownictwu zestawienie kwartalne z błędnymi sumami w dwóch tabelach. Poprawioną wersję trzeba było rozesłać ponownie.",
                },
                new EvaluationNote
                {
                    Id = 3, EvaluationId = 17, Criterion = Criterion.Reliability, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 3, 12, 10, 15, 0),
                    Text = "Przed wysłaniem raportu rocznego samodzielnie sprawdziła dane z rejestrem źródłowym i wykryła dwie rozbieżności, które poprawiła przed publikacją.",
                },
                new EvaluationNote
                {
                    Id = 4, EvaluationId = 17, Criterion = Criterion.Reliability, Tone = NoteTone.Negative, CreatedAt = new DateTime(2025, 9, 3, 15, 40, 0),
                    Text = "Przekazała zestawienie dla departamentu dwa dni po uzgodnionym terminie i nie uprzedziła wcześniej o opóźnieniu.",
                },
                new EvaluationNote
                {
                    Id = 5, EvaluationId = 17, Criterion = Criterion.Communication, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 6, 17, 11, 0, 0),
                    Text = "Przygotowała notatkę dla dyrektora, w której jasno oddzieliła ustalenia od kwestii otwartych. Dyrektor przyjął ją bez dopytywania.",
                },
                new EvaluationNote
                {
                    Id = 6, EvaluationId = 17, Criterion = Criterion.Cooperation, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 11, 5, 9, 20, 0),
                    Text = "Pomogła nowej osobie w wydziale przygotować pierwsze zestawienie i wyjaśniła jej zasady weryfikacji danych.",
                },
                new EvaluationNote
                {
                    Id = 7, EvaluationId = 17, Criterion = Criterion.AnalyticalThinking, Tone = NoteTone.Positive, CreatedAt = new DateTime(2026, 2, 24, 14, 5, 0),
                    Text = "Zauważyła, że spadek liczby wniosków wynika ze zmiany formularza, a nie z mniejszego zainteresowania, i pokazała to na danych z trzech kwartałów.",
                },
                new EvaluationNote
                {
                    Id = 8, EvaluationId = 17, Criterion = Criterion.WorkOrganization, Tone = NoteTone.Neutral, CreatedAt = new DateTime(2026, 5, 14, 16, 30, 0),
                    Text = "Zrealizowała plan kwartalny, ale dwa zadania wymagały przesunięcia terminu po uzgodnieniu z naczelnikiem.",
                },
                new EvaluationNote
                {
                    Id = 9, EvaluationId = 17, Criterion = Criterion.KnowledgeAndDevelopment, Tone = NoteTone.Positive, CreatedAt = new DateTime(2026, 9, 30, 10, 0, 0),
                    Text = "Ukończyła szkolenie z analizy danych i zastosowała nową metodę w raporcie kwartalnym. Pokazała ją też zespołowi na spotkaniu wydziału.",
                },
                new EvaluationNote
                {
                    Id = 10, EvaluationId = 15, Criterion = Criterion.Reliability, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 2, 11, 10, 30, 0),
                    Text = "Przy wniosku o dane z rejestru zawierającego dane osobowe sama ustaliła, które zasady anonimizacji mają zastosowanie, i przed wysyłką dodała drugą kontrolę zestawienia.",
                },
                new EvaluationNote
                {
                    Id = 11, EvaluationId = 15, Criterion = Criterion.KnowledgeAndDevelopment, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 5, 20, 14, 0, 0),
                    Text = "Przy nietypowym wniosku o dane z trzech rejestrów połączyła znajomość przepisów o dostępie do informacji publicznej z metodami łączenia danych i wyjaśniła, dlaczego części danych nie da się zestawić.",
                },
                new EvaluationNote
                {
                    Id = 12, EvaluationId = 15, Criterion = Criterion.Cooperation, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 9, 16, 9, 45, 0),
                    Text = "Z własnej inicjatywy przygotowała dla wydziału instrukcję obsługi wniosków o dane z rejestru i odpowiadała na pytania nowych osób. Po kolejnych pytaniach uzupełniła instrukcję o przykłady.",
                },
                new EvaluationNote
                {
                    Id = 13, EvaluationId = 15, Criterion = Criterion.Reliability, Tone = NoteTone.Negative, CreatedAt = new DateTime(2025, 10, 7, 15, 10, 0),
                    Text = "Dwie odpowiedzi na wnioski o dane przygotowała dopiero po przypomnieniu, w ostatnim dniu terminu. O tym, że czeka na dane z innej komórki, poinformowała dzień przed terminem.",
                },
                new EvaluationNote
                {
                    Id = 14, EvaluationId = 15, Criterion = Criterion.Communication, Tone = NoteTone.Neutral, CreatedAt = new DateTime(2026, 1, 21, 11, 20, 0),
                    Text = "Odpowiedzi na wnioski są poprawne i podają termin przekazania brakujących danych, ale przy złożonych sprawach wnioskodawcy dopytują o kontekst i znaczenie skrótów.",
                },
                new EvaluationNote
                {
                    Id = 15, EvaluationId = 15, Criterion = Criterion.KnowledgeAndDevelopment, Tone = NoteTone.Positive, CreatedAt = new DateTime(2026, 3, 10, 13, 0, 0),
                    Text = "Przed zmianą przepisów o ponownym wykorzystywaniu informacji sama zapisała się na szkolenie, a potem sprawdziła na kilku wnioskach, czy nowy wzór odpowiedzi skraca ich obsługę.",
                },
                new EvaluationNote
                {
                    Id = 16, EvaluationId = 15, Criterion = Criterion.WorkOrganization, Tone = NoteTone.Negative, CreatedAt = new DateTime(2026, 4, 15, 16, 0, 0),
                    Text = "Po pilnym zleceniu raportu dla ministerstwa przesunęła obsługę wniosków, ale o skutkach dla ich terminów poinformowała dopiero po pytaniu naczelnika.",
                },
                new EvaluationNote
                {
                    Id = 17, EvaluationId = 15, Criterion = Criterion.AnalyticalThinking, Tone = NoteTone.Positive, CreatedAt = new DateTime(2026, 6, 9, 10, 15, 0),
                    Text = "Zauważyła, że wzrost liczby wniosków o dane zbiega się ze zmianą strony BIP. Sprawdziła to na danych z dwóch lat i odrzuciła wyjaśnienie sezonowe.",
                },
                new EvaluationNote
                {
                    Id = 18, EvaluationId = 15, Criterion = Criterion.Communication, Tone = NoteTone.Positive, CreatedAt = new DateTime(2026, 8, 26, 12, 30, 0),
                    Text = "Na spotkaniu z Biurem Prawnym uzasadniła odmowę udostępnienia części danych, odpowiedziała na zastrzeżenia i wyjaśniła ograniczenia danych z rejestru.",
                },
                new EvaluationNote
                {
                    Id = 19, EvaluationId = 15, Criterion = Criterion.WorkOrganization, Tone = NoteTone.Positive, CreatedAt = new DateTime(2026, 9, 22, 9, 0, 0),
                    Text = "Przy trzech wnioskach z tym samym terminem porównała skutki opóźnienia każdego z nich i uzgodniła z naczelnikiem kolejność obsługi.",
                },
                new EvaluationNote
                {
                    Id = 20, EvaluationId = 17, Criterion = Criterion.WorkOrganization, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 1, 28, 10, 0, 0),
                    Text = "Przy raporcie rocznym rozpisała etapy z terminami cząstkowymi i punktami kontroli danych od trzech komórek. Zaplanowała też, co zrobi, jeśli któraś komórka spóźni się z danymi.",
                },
                new EvaluationNote
                {
                    Id = 21, EvaluationId = 17, Criterion = Criterion.Communication, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 4, 23, 13, 30, 0),
                    Text = "Na spotkaniu z Wydziałem Budżetu powtórzyła własnymi słowami stanowisko rozmówców i wyjaśniła, że obie strony inaczej rozumieją pojęcie „wydatki wykonane”.",
                },
                new EvaluationNote
                {
                    Id = 22, EvaluationId = 17, Criterion = Criterion.Creativity, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 7, 8, 11, 15, 0),
                    Text = "Zaproponowała zastąpienie ręcznego scalania arkuszy prostym skryptem. Wypróbowała go na jednym raporcie, porównała wyniki z wersją ręczną i poprawiła skrypt po uwagach zespołu.",
                },
                new EvaluationNote
                {
                    Id = 23, EvaluationId = 17, Criterion = Criterion.KnowledgeAndDevelopment, Tone = NoteTone.Positive, CreatedAt = new DateTime(2025, 10, 21, 9, 40, 0),
                    Text = "Do zestawienia o zatrudnieniu porównała dane z dwóch systemów kadrowych, wyjaśniła rozbieżności i wybrała źródło zgodne z zasadami ochrony danych.",
                },
                new EvaluationNote
                {
                    Id = 24, EvaluationId = 17, Criterion = Criterion.Cooperation, Tone = NoteTone.Positive, CreatedAt = new DateTime(2026, 1, 15, 14, 20, 0),
                    Text = "W podsumowaniu raportu rocznego wymieniła osoby z Wydziału Budżetu, które przygotowały dane źródłowe, i opisała, dlaczego ich praca była ważna dla wyniku.",
                },
                new EvaluationNote
                {
                    Id = 25, EvaluationId = 17, Criterion = Criterion.Reliability, Tone = NoteTone.Neutral, CreatedAt = new DateTime(2026, 3, 18, 15, 0, 0),
                    Text = "Kolejne zestawienia przekazywała w terminie, ale o zagrożeniu terminu raportu półrocznego poinformowała dopiero w dniu wysyłki.",
                },
                new EvaluationNote
                {
                    Id = 26, EvaluationId = 17, Criterion = Criterion.Creativity, Tone = NoteTone.Negative, CreatedAt = new DateTime(2026, 7, 1, 10, 30, 0),
                    Text = "W raporcie o skargach trzymała się dotychczasowego podziału na kategorie, mimo że nowe rodzaje skarg do nich nie pasowały. Zmieniła podział dopiero po sugestii naczelnika.",
                });
        });

        modelBuilder.Entity<Conversation>(c =>
        {
            c.ToTable("Conversations");
            c.Property(p => p.Type).HasConversion<string>().HasMaxLength(30);
            c.Property(p => p.Status).HasConversion<string>().HasMaxLength(30);
            c.HasIndex(p => new { p.EvaluationId, p.Type }).IsUnique();
            c.HasOne(p => p.Evaluation)
                .WithMany(p => p.Conversations)
                .HasForeignKey(p => p.EvaluationId)
                .OnDelete(DeleteBehavior.Cascade);

            c.HasData(
                new Conversation { Id = 1, EvaluationId = 6, Type = ConversationType.Starting, At = new DateTime(2024, 12, 2, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 2, EvaluationId = 6, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 26, 10, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 9, EvaluationId = 5, Type = ConversationType.Starting, At = new DateTime(2024, 11, 17, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 10, EvaluationId = 5, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 5, 13, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 17, EvaluationId = 9, Type = ConversationType.Starting, At = new DateTime(2026, 11, 19, 10, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 20, EvaluationId = 11, Type = ConversationType.Starting, At = new DateTime(2025, 2, 10, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 28, EvaluationId = 16, Type = ConversationType.Starting, At = new DateTime(2022, 12, 20, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 29, EvaluationId = 16, Type = ConversationType.Assessment, At = new DateTime(2024, 11, 28, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 30, EvaluationId = 17, Type = ConversationType.Starting, At = new DateTime(2024, 12, 18, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 31, EvaluationId = 17, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 19, 10, 0, 0), Status = ConversationStatus.Held });
        });

        modelBuilder.Entity<EvaluationAdditionalCriterion>(a =>
        {
            a.ToTable("EvaluationAdditionalCriteria");
            a.HasKey(p => new { p.EvaluationId, p.Criterion });
            a.Property(p => p.Criterion).HasConversion<string>().HasMaxLength(40);
            a.HasOne(p => p.Evaluation)
                .WithMany(p => p.AdditionalCriteria)
                .HasForeignKey(p => p.EvaluationId)
                .OnDelete(DeleteBehavior.Cascade);

            a.HasData(
                new EvaluationAdditionalCriterion { EvaluationId = 5, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 6, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 9, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 9, Criterion = Criterion.PublicSpeaking },
                new EvaluationAdditionalCriterion { EvaluationId = 11, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 16, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 17, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 17, Criterion = Criterion.Creativity });
        });

        modelBuilder.Entity<CompetencyIndicator>(i =>
        {
            i.ToTable("CompetencyIndicators");
            i.Property(p => p.Criterion).HasConversion<string>().HasMaxLength(40);
            i.Property(p => p.Text).HasMaxLength(1000);
            i.HasIndex(p => new { p.Criterion, p.Number }).IsUnique();
            i.HasData(CompetencyModelSource.Indicators.Select(x => new CompetencyIndicator
            {
                Id = x.Id, Criterion = x.Criterion, Number = x.Number, Text = x.Text,
            }));
        });

        modelBuilder.Entity<IndicatorBehavior>(b =>
        {
            b.ToTable("IndicatorBehaviors");
            b.HasKey(p => new { p.IndicatorId, p.Level });
            b.Property(p => p.Description).HasMaxLength(1000);
            b.HasOne(p => p.Indicator)
                .WithMany(p => p.Behaviors)
                .HasForeignKey(p => p.IndicatorId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasData(CompetencyModelSource.Behaviors.Select(x => new IndicatorBehavior
            {
                IndicatorId = x.IndicatorId, Level = x.Level, Description = x.Description,
            }));
        });

        modelBuilder.Entity<IndicatorRating>(r =>
        {
            r.ToTable("IndicatorRatings");
            r.HasKey(p => new { p.EvaluationId, p.IndicatorId });
            r.HasOne(p => p.Evaluation)
                .WithMany()
                .HasForeignKey(p => p.EvaluationId)
                .OnDelete(DeleteBehavior.Cascade);
            r.HasOne(p => p.Indicator)
                .WithMany()
                .HasForeignKey(p => p.IndicatorId)
                .OnDelete(DeleteBehavior.NoAction);

            r.HasData(
                DemoRatings(17, Criterion.KnowledgeAndDevelopment, 3, 4, 4)
                    .Concat(DemoRatings(17, Criterion.Reliability, 4, 2))
                    .Concat(DemoRatings(17, Criterion.WorkOrganization, 4, 3, null, 3, 3))
                    .Concat(DemoRatings(17, Criterion.Cooperation, 3, 3, 3, 4, 3, 3))
                    .Concat(DemoRatings(17, Criterion.Communication, 4, 3, 4, 3))
                    .Concat(DemoRatings(17, Criterion.AnalyticalThinking, 4, 4, 3))
                    .Concat(DemoRatings(17, Criterion.Creativity, 4, 2)));
        });

        modelBuilder.Entity<CriterionDecision>(d =>
        {
            d.ToTable("CriterionDecisions");
            d.HasKey(p => new { p.EvaluationId, p.Criterion });
            d.Property(p => p.Criterion).HasConversion<string>().HasMaxLength(40);
            d.Property(p => p.Justification).HasMaxLength(CriterionDecision.MaxJustificationLength);
            d.HasOne(p => p.Evaluation)
                .WithMany()
                .HasForeignKey(p => p.EvaluationId)
                .OnDelete(DeleteBehavior.Cascade);

            d.HasData(
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.WorkOrganization, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.Cooperation, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.Communication, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 5, Level = 5, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.Cooperation, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.Communication, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 17, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 20, 10, 0, 0) },
                new CriterionDecision { EvaluationId = 17, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 20, 10, 0, 0) },
                new CriterionDecision { EvaluationId = 17, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 20, 10, 0, 0) },
                new CriterionDecision { EvaluationId = 17, Criterion = Criterion.Cooperation, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 20, 10, 0, 0) },
                new CriterionDecision { EvaluationId = 17, Criterion = Criterion.Communication, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 20, 10, 0, 0) },
                new CriterionDecision { EvaluationId = 17, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 20, 10, 0, 0) },
                new CriterionDecision { EvaluationId = 17, Criterion = Criterion.Creativity, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 20, 10, 0, 0) });
        });
    }

    private static string Lines(params string[] lines) => string.Join('\n', lines);

    private static IEnumerable<IndicatorRating> DemoRatings(int evaluationId, Criterion criterion, params int?[] levels)
    {
        var indicators = CompetencyModelSource.Indicators.Where(i => i.Criterion == criterion).OrderBy(i => i.Number).ToList();
        if (indicators.Count != levels.Length)
        {
            throw new InvalidOperationException($"Kryterium {criterion} ma {indicators.Count} wskaźników, a dane demo podają {levels.Length} odpowiedzi.");
        }
        return indicators.Zip(levels, (indicator, level) => new IndicatorRating { EvaluationId = evaluationId, IndicatorId = indicator.Id, Level = level });
    }
}
