using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<EmployeeSupervisor> EmployeeSupervisors => Set<EmployeeSupervisor>();

    public DbSet<Evaluation> Evaluations => Set<Evaluation>();

    public DbSet<JobDescription> JobDescriptions => Set<JobDescription>();

    public DbSet<EvaluationNote> EvaluationNotes => Set<EvaluationNote>();

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
                new Employee { Id = 11, FirstName = "Paweł", LastName = "Kozłowski", Position = "Specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2026, 7, 1) },
                new Employee { Id = 12, FirstName = "Karolina", LastName = "Mazur", Position = "Specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2021, 12, 20) },
                new Employee { Id = 13, FirstName = "Łukasz", LastName = "Grabowski", Position = "Specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2020, 12, 10) },
                new Employee { Id = 14, FirstName = "Natalia", LastName = "Krawczyk", Position = "Główny specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2023, 4, 3) },
                new Employee { Id = 15, FirstName = "Bartosz", LastName = "Wróbel", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2022, 1, 17) },
                new Employee { Id = 16, FirstName = "Monika", LastName = "Sikora", Position = "Specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2019, 11, 28) });
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
                new EmployeeSupervisor { EmployeeId = 13, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 14, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 15, SupervisorId = 3 },
                new EmployeeSupervisor { EmployeeId = 16, SupervisorId = 3 });
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
                    EmployeeId = 12,
                    Purpose = "Realizacja zadań związanych z analizą danych i przygotowywaniem informacji dla kierownictwa.",
                    Tasks = Lines(
                        "Przygotowywanie cyklicznych raportów z danych wydziału",
                        "Analiza danych ankietowych i statystycznych",
                        "Przygotowywanie projektów odpowiedzi na pisma",
                        "Aktualizacja danych publikowanych w serwisie urzędu"),
                    RequiredCompetencies = Lines("Myślenie analityczne", "Rzetelność", "Komunikacja pisemna"),
                },
                new JobDescription
                {
                    EmployeeId = 14,
                    Purpose = "Projektowanie i utrzymanie zestawów wskaźników wydziału oraz zapewnienie spójności metod analizy danych.",
                    Tasks = Lines(
                        "Projektowanie wskaźników i metod pomiaru dla zadań departamentu",
                        "Przygotowywanie analiz porównawczych na potrzeby kierownictwa",
                        "Opracowywanie standardów opisu i weryfikacji źródeł danych",
                        "Koordynowanie prac analitycznych realizowanych przez kilka osób wydziału"),
                    RequiredCompetencies = Lines("Myślenie analityczne", "Orientacja na cele", "Współpraca", "Komunikacja pisemna"),
                },
                new JobDescription
                {
                    EmployeeId = 15,
                    Purpose = "Przygotowywanie analiz statystycznych i prognoz na potrzeby planowania zadań departamentu.",
                    Tasks = Lines(
                        "Przygotowywanie prognoz i analiz trendów na podstawie danych wydziału",
                        "Opracowywanie zestawień do planów i sprawozdań departamentu",
                        "Weryfikacja poprawności obliczeń w raportach przygotowywanych przez wydział",
                        "Udzielanie wyjaśnień dotyczących metod analizy innym komórkom"),
                    RequiredCompetencies = Lines("Myślenie analityczne", "Rzetelność", "Komunikacja", "Orientacja na cele"),
                },
                new JobDescription
                {
                    EmployeeId = 16,
                    Purpose = "Obsługa wniosków o udostępnienie danych i przygotowywanie zestawień dla podmiotów zewnętrznych.",
                    Tasks = Lines(
                        "Rozpatrywanie wniosków o udostępnienie danych wydziału",
                        "Przygotowywanie zestawień danych dla podmiotów zewnętrznych",
                        "Prowadzenie rejestru udostępnionych danych",
                        "Współpraca z działem prawnym przy odpowiedziach na wnioski"),
                    RequiredCompetencies = Lines("Rzetelność", "Komunikacja pisemna", "Współpraca"),
                });
        });

        modelBuilder.Entity<Evaluation>(v =>
        {
            v.ToTable("Evaluations");
            v.Property(p => p.Status).HasConversion<string>().HasMaxLength(30);
            v.Property(p => p.Sheet).HasConversion<string>().HasMaxLength(1);
            v.Property(p => p.ConversationStatus).HasConversion<string>().HasMaxLength(30);
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
                    Id = 1, EmployeeId = 1, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2024, 12, 18, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 12, 15), PeriodEnd = new DateOnly(2025, 12, 14), DueDate = new DateOnly(2025, 12, 15),
                    ConversationAt = new DateTime(2025, 12, 8, 10, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 11, 20, 9, 0, 0), CompletedAt = new DateTime(2025, 12, 10, 14, 0, 0),
                },
                new Evaluation
                {
                    Id = 2, EmployeeId = 1, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.InProgress, SheetConfirmedAt = new DateTime(2025, 12, 18, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 12, 15), PeriodEnd = new DateOnly(2026, 12, 14), DueDate = new DateOnly(2026, 12, 15),
                    ConversationAt = new DateTime(2026, 11, 27, 10, 0, 0), ConversationStatus = ConversationStatus.Scheduled,
                    StartedAt = new DateTime(2026, 11, 12, 10, 30, 0),
                },
                new Evaluation
                {
                    Id = 3, EmployeeId = 2, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2024, 11, 5, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 11, 2), PeriodEnd = new DateOnly(2025, 11, 1), DueDate = new DateOnly(2025, 11, 2),
                    ConversationAt = new DateTime(2025, 10, 27, 11, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 10, 6, 9, 0, 0), CompletedAt = new DateTime(2025, 10, 28, 12, 0, 0),
                },
                new Evaluation
                {
                    Id = 4, EmployeeId = 9, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2025, 1, 14, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 1, 10), PeriodEnd = new DateOnly(2026, 1, 9), DueDate = new DateOnly(2026, 1, 10),
                    ConversationAt = new DateTime(2026, 1, 7, 9, 30, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 12, 15, 9, 0, 0), CompletedAt = new DateTime(2026, 1, 8, 11, 0, 0),
                },
                new Evaluation
                {
                    Id = 5, EmployeeId = 10, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2025, 6, 4, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 6, 1), PeriodEnd = new DateOnly(2026, 5, 31), DueDate = new DateOnly(2026, 6, 1),
                    ConversationAt = new DateTime(2026, 5, 26, 13, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2026, 5, 4, 9, 0, 0), CompletedAt = new DateTime(2026, 5, 27, 13, 0, 0),
                },
                new Evaluation
                {
                    Id = 6, EmployeeId = 9, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2026, 1, 13, 9, 0, 0),
                    PeriodStart = new DateOnly(2026, 1, 10), PeriodEnd = new DateOnly(2027, 1, 9), DueDate = new DateOnly(2027, 1, 10),
                    ConversationAt = new DateTime(2026, 12, 18, 9, 0, 0), ConversationStatus = ConversationStatus.Scheduled,
                    StartedAt = new DateTime(2026, 11, 16, 14, 0, 0),
                },
                new Evaluation
                {
                    Id = 7, EmployeeId = 12, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2024, 12, 23, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 12, 20), PeriodEnd = new DateOnly(2025, 12, 19), DueDate = new DateOnly(2025, 12, 20),
                    ConversationAt = new DateTime(2025, 12, 17, 12, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 11, 24, 9, 0, 0), CompletedAt = new DateTime(2025, 12, 18, 10, 0, 0),
                },
                new Evaluation
                {
                    Id = 8, EmployeeId = 13, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.NotStarted, SheetConfirmedAt = new DateTime(2025, 12, 12, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 12, 10), PeriodEnd = new DateOnly(2026, 12, 9), DueDate = new DateOnly(2026, 12, 10),
                    ConversationAt = new DateTime(2026, 11, 14, 10, 0, 0), ConversationStatus = ConversationStatus.Scheduled,
                },
                new Evaluation
                {
                    Id = 9, EmployeeId = 14, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2025, 4, 8, 9, 0, 0),
                    CriteriaConfirmedAt = new DateTime(2025, 4, 22, 10, 0, 0),
                    PeriodStart = new DateOnly(2025, 4, 3), PeriodEnd = new DateOnly(2026, 4, 2), DueDate = new DateOnly(2026, 4, 3),
                    ConversationAt = new DateTime(2026, 3, 25, 10, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2026, 3, 2, 9, 0, 0), CompletedAt = new DateTime(2026, 3, 30, 12, 0, 0),
                },
                new Evaluation
                {
                    Id = 10, EmployeeId = 14, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2026, 4, 7, 9, 0, 0),
                    CriteriaConfirmedAt = new DateTime(2026, 4, 21, 10, 0, 0),
                    PeriodStart = new DateOnly(2026, 4, 3), PeriodEnd = new DateOnly(2027, 4, 2), DueDate = new DateOnly(2027, 4, 3),
                },
                new Evaluation
                {
                    Id = 11, EmployeeId = 15, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2025, 1, 21, 9, 0, 0),
                    CriteriaConfirmedAt = new DateTime(2025, 2, 4, 10, 0, 0),
                    PeriodStart = new DateOnly(2025, 1, 17), PeriodEnd = new DateOnly(2026, 1, 16), DueDate = new DateOnly(2026, 1, 17),
                    ConversationAt = new DateTime(2026, 1, 12, 11, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 12, 1, 9, 0, 0), CompletedAt = new DateTime(2026, 1, 14, 12, 0, 0),
                },
                new Evaluation
                {
                    Id = 12, EmployeeId = 15, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2026, 1, 20, 9, 0, 0),
                    CriteriaConfirmedAt = new DateTime(2026, 2, 3, 10, 0, 0),
                    PeriodStart = new DateOnly(2026, 1, 17), PeriodEnd = new DateOnly(2027, 1, 16), DueDate = new DateOnly(2027, 1, 17),
                },
                new Evaluation
                {
                    Id = 13, EmployeeId = 16, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2024, 12, 3, 9, 0, 0),
                    CriteriaConfirmedAt = new DateTime(2024, 12, 17, 10, 0, 0), WrittenEvaluationAt = new DateTime(2025, 11, 20, 13, 0, 0),
                    PeriodStart = new DateOnly(2024, 11, 28), PeriodEnd = new DateOnly(2025, 11, 27), DueDate = new DateOnly(2025, 11, 28),
                    ConversationAt = new DateTime(2025, 11, 17, 10, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 10, 1, 9, 0, 0), CompletedAt = new DateTime(2025, 11, 28, 12, 0, 0),
                },
                new Evaluation
                {
                    Id = 14, EmployeeId = 16, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.InProgress, SheetConfirmedAt = new DateTime(2025, 12, 2, 9, 0, 0),
                    CriteriaConfirmedAt = new DateTime(2025, 12, 16, 10, 0, 0), WrittenEvaluationAt = new DateTime(2026, 11, 19, 14, 30, 0),
                    PeriodStart = new DateOnly(2025, 11, 28), PeriodEnd = new DateOnly(2026, 11, 27), DueDate = new DateOnly(2026, 11, 28),
                    ConversationAt = new DateTime(2026, 11, 16, 11, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2026, 10, 1, 9, 0, 0),
                });
        });

        modelBuilder.Entity<EvaluationNote>(n =>
        {
            n.ToTable("EvaluationNotes");
            n.Property(p => p.Text).HasMaxLength(EvaluationNote.MaxTextLength);
            n.HasIndex(p => new { p.EvaluationId, p.CreatedAt });
            n.HasOne(p => p.Evaluation)
                .WithMany()
                .HasForeignKey(p => p.EvaluationId)
                .OnDelete(DeleteBehavior.Cascade);

            n.HasData(
                new EvaluationNote
                {
                    Id = 1, EvaluationId = 10, CreatedAt = new DateTime(2026, 5, 14, 15, 20, 0),
                    Text = "Przygotowała zestaw wskaźników do sprawozdania kwartalnego tydzień przed terminem. Kierownictwo departamentu przyjęło go bez poprawek.",
                },
                new EvaluationNote
                {
                    Id = 2, EvaluationId = 10, CreatedAt = new DateTime(2026, 9, 8, 11, 0, 0),
                    Text = "Z własnej inicjatywy poprowadziła dla wydziału warsztat z weryfikacji źródeł danych. Uczestnicy ocenili go jako przydatny.",
                },
                new EvaluationNote
                {
                    Id = 3, EvaluationId = 12, CreatedAt = new DateTime(2026, 3, 10, 9, 45, 0),
                    Text = "Prognoza zatrudnienia na 2027 r. wymagała dwóch poprawek metodologicznych. Po wskazówkach przygotował poprawioną wersję w terminie.",
                },
                new EvaluationNote
                {
                    Id = 4, EvaluationId = 12, CreatedAt = new DateTime(2026, 10, 2, 14, 10, 0),
                    Text = "Samodzielnie wykrył błąd w obliczeniach raportu rocznego przed jego wysłaniem do dyrektora i zaproponował dodatkowy krok kontroli.",
                },
                new EvaluationNote
                {
                    Id = 5, EvaluationId = 14, CreatedAt = new DateTime(2026, 3, 4, 10, 15, 0),
                    Text = "Rozpatrzyła wszystkie wnioski z pierwszego kwartału w terminie, w tym dwa wymagające uzgodnień z działem prawnym.",
                },
                new EvaluationNote
                {
                    Id = 6, EvaluationId = 14, CreatedAt = new DateTime(2026, 8, 21, 13, 40, 0),
                    Text = "Uporządkowała rejestr udostępnionych danych i zaproponowała wzór odpowiedzi skracający obsługę typowych wniosków.",
                });
        });
    }

    private static string Lines(params string[] lines) => string.Join('\n', lines);
}
