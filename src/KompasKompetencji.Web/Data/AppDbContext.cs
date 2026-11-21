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

    public DbSet<JobDescription> JobDescriptions => Set<JobDescription>();

    public DbSet<AppUser> Users => Set<AppUser>();

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

            e.HasData(
                new Employee { Id = 1, FirstName = "Anna", LastName = "Kowalska", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2019, 12, 15) },
                new Employee { Id = 2, FirstName = "Jan", LastName = "Nowak", Position = "Specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2022, 11, 2) },
                new Employee { Id = 3, FirstName = "Ewa", LastName = "Wiśniewska", Position = "Naczelnik wydziału", Department = "Wydział Analiz Danych", IsSupervisor = true, EmployedSince = new DateOnly(2015, 4, 1) },
                new Employee { Id = 4, FirstName = "Tomasz", LastName = "Zieliński", Position = "Dyrektor departamentu", Department = "Departament Danych i Usług Cyfrowych", IsSupervisor = true, EmployedSince = new DateOnly(2012, 9, 1) },
                new Employee { Id = 5, FirstName = "Marek", LastName = "Lewandowski", Position = "Naczelnik wydziału", Department = "Wydział Usług Cyfrowych", IsSupervisor = true, EmployedSince = new DateOnly(2016, 2, 15) },
                new Employee { Id = 6, FirstName = "Magdalena", LastName = "Dąbrowska", Position = "Zastępca naczelnika wydziału", Department = "Wydział Usług Cyfrowych", IsSupervisor = true, EmployedSince = new DateOnly(2018, 5, 1) },
                new Employee { Id = 7, FirstName = "Katarzyna", LastName = "Wójcik", Position = "Główny specjalista", Department = "Wydział Usług Cyfrowych", EmployedSince = new DateOnly(2021, 9, 1) },
                new Employee { Id = 8, FirstName = "Piotr", LastName = "Kamiński", Position = "Starszy referent", Department = "Wydział Usług Cyfrowych", EmployedSince = new DateOnly(2024, 3, 1) },
                new Employee { Id = 9, FirstName = "Michał", LastName = "Szymański", Position = "Referent", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2018, 1, 10) },
                new Employee { Id = 10, FirstName = "Agnieszka", LastName = "Woźniak", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2020, 6, 1) },
                new Employee { Id = 11, FirstName = "Paweł", LastName = "Kozłowski", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2026, 11, 5) },
                new Employee { Id = 12, FirstName = "Karolina", LastName = "Mazur", Position = "Specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2021, 12, 20) },
                new Employee { Id = 13, FirstName = "Łukasz", LastName = "Grabowski", Position = "Specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2020, 12, 10) });
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
                new EmployeeSupervisor { EmployeeId = 1, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 2, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 3, SupervisorId = 4 },
                new EmployeeSupervisor { EmployeeId = 5, SupervisorId = 4 },
                new EmployeeSupervisor { EmployeeId = 6, SupervisorId = 5 },
                new EmployeeSupervisor { EmployeeId = 7, SupervisorId = 5 },
                new EmployeeSupervisor { EmployeeId = 8, SupervisorId = 6 },
                new EmployeeSupervisor { EmployeeId = 8, SupervisorId = 5 },
                new EmployeeSupervisor { EmployeeId = 9, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 10, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 11, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 12, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 13, SupervisorId = 3 });
        });

        modelBuilder.Entity<JobDescription>(j =>
        {
            j.ToTable("JobDescriptions");
            j.HasKey(p => p.EmployeeId);
            j.Property(p => p.Purpose).HasMaxLength(1000);
            j.Property(p => p.Tasks).HasMaxLength(4000);
            j.Property(p => p.RequiredCompetencies).HasMaxLength(1000);
            j.HasOne(p => p.Employee)
                .WithOne(p => p.JobDescription)
                .HasForeignKey<JobDescription>(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            j.HasData(
                new JobDescription
                {
                    EmployeeId = 1,
                    Purpose = "Przygotowywanie analiz danych wspierających decyzje kierownictwa departamentu oraz dbanie o jakość danych wykorzystywanych w raportach.",
                    Tasks = Lines(
                        "Przygotowywanie analiz i raportów na potrzeby kierownictwa departamentu",
                        "Opracowywanie wskaźników i metod pomiaru realizacji zadań",
                        "Weryfikacja jakości i kompletności danych z systemów dziedzinowych",
                        "Przygotowywanie projektów stanowisk do dokumentów przekazanych do zaopiniowania",
                        "Wsparcie merytoryczne mniej doświadczonych pracowników wydziału"),
                    RequiredCompetencies = Lines("Myślenie analityczne", "Orientacja na cele", "Komunikacja pisemna", "Współpraca"),
                },
                new JobDescription
                {
                    EmployeeId = 2,
                    Purpose = "Realizacja zadań związanych z gromadzeniem, przetwarzaniem i udostępnianiem danych na potrzeby wydziału.",
                    Tasks = Lines(
                        "Gromadzenie i porządkowanie danych z jednostek podległych",
                        "Przygotowywanie zestawień i wizualizacji danych",
                        "Udzielanie odpowiedzi na zapytania dotyczące danych publicznych",
                        "Prowadzenie dokumentacji źródeł danych"),
                    RequiredCompetencies = Lines("Myślenie analityczne", "Rzetelność", "Komunikacja", "Współpraca"),
                },
                new JobDescription
                {
                    EmployeeId = 3,
                    Purpose = "Kierowanie Wydziałem Analiz Danych, zapewnienie terminowej i rzetelnej realizacji zadań wydziału oraz rozwój kompetencji podległych pracowników.",
                    Tasks = Lines(
                        "Planowanie, organizowanie i nadzorowanie pracy wydziału",
                        "Przydzielanie zadań i ocena ich realizacji przez pracowników",
                        "Przygotowywanie stanowisk i rekomendacji dla dyrektora departamentu",
                        "Współpraca z innymi komórkami organizacyjnymi i instytucjami",
                        "Prowadzenie ocen okresowych i planowanie rozwoju zawodowego pracowników"),
                    RequiredCompetencies = Lines("Zarządzanie zespołem", "Podejmowanie decyzji", "Myślenie strategiczne", "Zarządzanie rozwojem pracowników", "Komunikacja"),
                    SubordinatesCount = 6,
                },
                new JobDescription
                {
                    EmployeeId = 9,
                    Purpose = "Obsługa administracyjna wydziału i wsparcie pracowników w przygotowywaniu zestawień danych.",
                    Tasks = Lines(
                        "Prowadzenie rejestru korespondencji wydziału",
                        "Przygotowywanie prostych zestawień danych według wskazówek",
                        "Archiwizowanie dokumentacji wydziału",
                        "Organizacja spotkań i przygotowywanie notatek"),
                    RequiredCompetencies = Lines("Rzetelność", "Organizacja pracy własnej", "Komunikacja"),
                },
                new JobDescription
                {
                    EmployeeId = 10,
                    Purpose = "Przygotowywanie i opiniowanie analiz dotyczących usług cyfrowych oraz koordynacja współpracy z dostawcami danych.",
                    Tasks = Lines(
                        "Opracowywanie analiz wpływu planowanych zmian na usługi cyfrowe",
                        "Opiniowanie projektów dokumentów pod kątem danych i wskaźników",
                        "Koordynowanie współpracy z instytucjami przekazującymi dane",
                        "Prowadzenie spotkań roboczych z udziałem innych komórek",
                        "Przygotowywanie materiałów na posiedzenia zespołów międzyresortowych"),
                    RequiredCompetencies = Lines("Myślenie analityczne", "Współpraca", "Komunikacja", "Orientacja na cele"),
                },
                new JobDescription
                {
                    EmployeeId = 11,
                    Purpose = "Prowadzenie spraw dotyczących reagowania na incydenty i sytuacje kryzysowe w zakresie danych i usług cyfrowych departamentu.",
                    Tasks = Lines(
                        "Udział w opracowaniu i aktualizacji planu reagowania na incydenty dotyczące danych i usług cyfrowych",
                        "Analiza zgłoszonych incydentów i przygotowywanie rekomendacji działań naprawczych",
                        "Współpraca z zespołami reagowania na incydenty w innych urzędach i instytucjach",
                        "Udział w przygotowaniu i przeprowadzaniu ćwiczeń z reagowania na sytuacje kryzysowe",
                        "Prowadzenie kontroli przestrzegania procedur bezpieczeństwa danych w jednostkach podległych"),
                    RequiredCompetencies = Lines(
                        "Organizacja pracy i orientacja na osiąganie celów",
                        "Współpraca",
                        "Myślenie analityczne",
                        "Wykorzystywanie wiedzy i doskonalenie zawodowe",
                        "Radzenie sobie w sytuacjach kryzysowych"),
                },
                new JobDescription
                {
                    EmployeeId = 12,
                    Purpose = "Realizacja zadań związanych z analizą danych i przygotowywaniem informacji dla kierownictwa.",
                    Tasks = Lines(
                        "Przygotowywanie cyklicznych raportów z danych wydziału",
                        "Analiza danych ankietowych i statystycznych",
                        "Przygotowywanie projektów odpowiedzi na pisma",
                        "Aktualizacja danych publikowanych w serwisie urzędu"),
                    RequiredCompetencies = Lines("Myślenie analityczne", "Rzetelność", "Komunikacja pisemna"),
                });
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
                    Id = 1, EmployeeId = 1, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2024, 12, 11, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 12, 16), PeriodEnd = new DateOnly(2025, 12, 14), DueDate = new DateOnly(2025, 12, 15),
                    Expectations = "Samodzielne przygotowywanie analiz dla kierownictwa w uzgodnionych terminach, z opisem źródeł i ograniczeń danych.",
                    PartIConfirmedAt = new DateTime(2024, 12, 16, 15, 0, 0), EmployeeAcquaintedOn = new DateOnly(2024, 12, 16),
                    StartedAt = new DateTime(2025, 11, 20, 9, 0, 0), CompletedAt = new DateTime(2025, 12, 10, 14, 0, 0),
                },
                new Evaluation
                {
                    Id = 2, EmployeeId = 1, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.InProgress, SheetConfirmedAt = new DateTime(2025, 12, 11, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 12, 16), PeriodEnd = new DateOnly(2026, 12, 14), DueDate = new DateOnly(2026, 12, 15),
                    Expectations = "Analizy dla kierownictwa oddawane w terminie i bez poprawek merytorycznych. Propozycje nowych wskaźników zgłaszane z własnej inicjatywy, co najmniej raz na kwartał.",
                    Goals = "Opracowanie zestawu wskaźników realizacji zadań departamentu do końca II kwartału 2026 r.",
                    PartIConfirmedAt = new DateTime(2025, 12, 16, 15, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 12, 16),
                    StartedAt = new DateTime(2026, 11, 12, 10, 30, 0),
                },
                new Evaluation
                {
                    Id = 3, EmployeeId = 2, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2024, 11, 5, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 11, 7), PeriodEnd = new DateOnly(2025, 11, 1), DueDate = new DateOnly(2025, 11, 2),
                    Expectations = "Zestawienia danych przygotowywane według przyjętego wzoru i sprawdzone przed przekazaniem.",
                    PartIConfirmedAt = new DateTime(2024, 11, 7, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2024, 11, 7),
                    StartedAt = new DateTime(2025, 10, 6, 9, 0, 0), CompletedAt = new DateTime(2025, 10, 28, 12, 0, 0),
                },
                new Evaluation
                {
                    Id = 4, EmployeeId = 9, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2025, 1, 14, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 1, 16), PeriodEnd = new DateOnly(2026, 1, 9), DueDate = new DateOnly(2026, 1, 10),
                    Expectations = "Korespondencja wydziału rejestrowana na bieżąco, odpowiedzi na proste pytania udzielane w ciągu dwóch dni.",
                    PartIConfirmedAt = new DateTime(2025, 1, 16, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 1, 16),
                    StartedAt = new DateTime(2025, 12, 15, 9, 0, 0), CompletedAt = new DateTime(2026, 1, 8, 11, 0, 0),
                },
                new Evaluation
                {
                    Id = 5, EmployeeId = 10, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2025, 11, 12, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 11, 17), PeriodEnd = new DateOnly(2026, 11, 9), DueDate = new DateOnly(2026, 11, 10),
                    Expectations = "Opinie do projektów dokumentów przygotowywane w terminie, z konkretnymi propozycjami zmian wskaźników.",
                    PartIConfirmedAt = new DateTime(2025, 11, 17, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 11, 17),
                    StartedAt = new DateTime(2026, 10, 12, 9, 0, 0), CompletedAt = new DateTime(2026, 11, 6, 13, 0, 0),
                },
                new Evaluation
                {
                    Id = 6, EmployeeId = 9, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2026, 1, 13, 9, 0, 0),
                    PeriodStart = new DateOnly(2026, 1, 15), PeriodEnd = new DateOnly(2027, 1, 9), DueDate = new DateOnly(2027, 1, 10),
                    Expectations = "Rejestr korespondencji prowadzony bez zaległości. Zestawienia danych przygotowywane samodzielnie według wskazówek, bez konieczności poprawek.",
                    PartIConfirmedAt = new DateTime(2026, 1, 15, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2026, 1, 15),
                    StartedAt = new DateTime(2026, 11, 16, 14, 0, 0),
                },
                new Evaluation
                {
                    Id = 7, EmployeeId = 12, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2024, 12, 23, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 1, 8), PeriodEnd = new DateOnly(2025, 12, 19), DueDate = new DateOnly(2025, 12, 20),
                    Expectations = "Cykliczne raporty przekazywane w terminie, z krótkim omówieniem najważniejszych zmian w danych.",
                    PartIConfirmedAt = new DateTime(2025, 1, 8, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 1, 8),
                    StartedAt = new DateTime(2025, 11, 24, 9, 0, 0), CompletedAt = new DateTime(2025, 12, 18, 10, 0, 0),
                },
                new Evaluation
                {
                    Id = 8, EmployeeId = 13, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.NotStarted, SheetConfirmedAt = new DateTime(2025, 12, 12, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 12, 17), PeriodEnd = new DateOnly(2026, 12, 9), DueDate = new DateOnly(2026, 12, 10),
                    Expectations = "Analizy danych przygotowywane samodzielnie, z wnioskami zrozumiałymi dla odbiorców spoza wydziału.",
                    PartIConfirmedAt = new DateTime(2025, 12, 17, 15, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 12, 17),
                },
                new Evaluation
                {
                    Id = 9, EmployeeId = 10, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.NotStarted, SheetConfirmedAt = new DateTime(2026, 11, 12, 9, 0, 0),
                    PeriodStart = new DateOnly(2026, 11, 6), PeriodEnd = new DateOnly(2027, 11, 9), DueDate = new DateOnly(2027, 11, 10),
                    Expectations = "Analizy wpływu zmian na usługi cyfrowe z rekomendacją i oceną ryzyka. Samodzielne przedstawianie stanowiska wydziału na posiedzeniach zespołów międzyresortowych.",
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
                new Conversation { Id = 1, EvaluationId = 1, Type = ConversationType.Starting, At = new DateTime(2024, 12, 16, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 2, EvaluationId = 1, Type = ConversationType.Assessment, At = new DateTime(2025, 12, 8, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 3, EvaluationId = 2, Type = ConversationType.Starting, At = new DateTime(2025, 12, 16, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 4, EvaluationId = 2, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 27, 10, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 5, EvaluationId = 3, Type = ConversationType.Starting, At = new DateTime(2024, 11, 7, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 6, EvaluationId = 3, Type = ConversationType.Assessment, At = new DateTime(2025, 10, 27, 11, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 7, EvaluationId = 4, Type = ConversationType.Starting, At = new DateTime(2025, 1, 16, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 8, EvaluationId = 4, Type = ConversationType.Assessment, At = new DateTime(2026, 1, 7, 9, 30, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 9, EvaluationId = 5, Type = ConversationType.Starting, At = new DateTime(2025, 11, 17, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 10, EvaluationId = 5, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 5, 13, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 11, EvaluationId = 6, Type = ConversationType.Starting, At = new DateTime(2026, 1, 15, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 12, EvaluationId = 6, Type = ConversationType.Assessment, At = new DateTime(2026, 12, 18, 9, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 13, EvaluationId = 7, Type = ConversationType.Starting, At = new DateTime(2025, 1, 8, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 14, EvaluationId = 7, Type = ConversationType.Assessment, At = new DateTime(2025, 12, 17, 12, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 15, EvaluationId = 8, Type = ConversationType.Starting, At = new DateTime(2025, 12, 17, 11, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 16, EvaluationId = 8, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 14, 10, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 17, EvaluationId = 9, Type = ConversationType.Starting, At = new DateTime(2026, 11, 26, 10, 0, 0), Status = ConversationStatus.Scheduled });
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
                new EvaluationAdditionalCriterion { EvaluationId = 1, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 2, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 2, Criterion = Criterion.Creativity },
                new EvaluationAdditionalCriterion { EvaluationId = 3, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 4, Criterion = Criterion.CustomerOrientation },
                new EvaluationAdditionalCriterion { EvaluationId = 5, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 6, Criterion = Criterion.CustomerOrientation },
                new EvaluationAdditionalCriterion { EvaluationId = 6, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 7, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 8, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 9, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 9, Criterion = Criterion.PublicSpeaking });
        });
    }

    private static string Lines(params string[] lines) => string.Join('\n', lines);
}
