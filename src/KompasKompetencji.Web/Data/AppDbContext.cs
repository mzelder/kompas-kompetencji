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
                new Employee { Id = 1, FirstName = "Anna", LastName = "Kowalska", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", JobPositionId = 1, EmployedSince = new DateOnly(2019, 12, 15) },
                new Employee { Id = 2, FirstName = "Jan", LastName = "Nowak", Position = "Specjalista", Department = "Wydział Analiz Danych", JobPositionId = 2, EmployedSince = new DateOnly(2022, 11, 2) },
                new Employee { Id = 3, FirstName = "Ewa", LastName = "Wiśniewska", Position = "Naczelnik wydziału", Department = "Wydział Analiz Danych", IsSupervisor = true, JobPositionId = 3, EmployedSince = new DateOnly(2015, 4, 1) },
                new Employee { Id = 4, FirstName = "Tomasz", LastName = "Zieliński", Position = "Dyrektor departamentu", Department = "Departament Danych i Usług Cyfrowych", IsSupervisor = true, EmployedSince = new DateOnly(2012, 9, 1) },
                new Employee { Id = 5, FirstName = "Marek", LastName = "Lewandowski", Position = "Naczelnik wydziału", Department = "Wydział Usług Cyfrowych", IsSupervisor = true, EmployedSince = new DateOnly(2016, 2, 15) },
                new Employee { Id = 6, FirstName = "Magdalena", LastName = "Dąbrowska", Position = "Zastępca naczelnika wydziału", Department = "Wydział Usług Cyfrowych", IsSupervisor = true, EmployedSince = new DateOnly(2018, 5, 1) },
                new Employee { Id = 7, FirstName = "Katarzyna", LastName = "Wójcik", Position = "Główny specjalista", Department = "Wydział Usług Cyfrowych", EmployedSince = new DateOnly(2021, 9, 1) },
                new Employee { Id = 8, FirstName = "Piotr", LastName = "Kamiński", Position = "Starszy referent", Department = "Wydział Usług Cyfrowych", EmployedSince = new DateOnly(2024, 3, 1) },
                new Employee { Id = 9, FirstName = "Michał", LastName = "Szymański", Position = "Referent", Department = "Wydział Analiz Danych", JobPositionId = 4, EmployedSince = new DateOnly(2018, 1, 10) },
                new Employee { Id = 10, FirstName = "Agnieszka", LastName = "Woźniak", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", JobPositionId = 5, EmployedSince = new DateOnly(2020, 6, 1) },
                new Employee { Id = 11, FirstName = "Paweł", LastName = "Kozłowski", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", JobPositionId = 6, EmployedSince = new DateOnly(2026, 11, 5) },
                new Employee { Id = 12, FirstName = "Karolina", LastName = "Mazur", Position = "Specjalista", Department = "Wydział Analiz Danych", JobPositionId = 7, EmployedSince = new DateOnly(2021, 12, 20) },
                new Employee { Id = 13, FirstName = "Łukasz", LastName = "Grabowski", Position = "Specjalista", Department = "Wydział Analiz Danych", EmployedSince = new DateOnly(2024, 12, 10) },
                new Employee { Id = 14, FirstName = "Natalia", LastName = "Krawczyk", Position = "Główny specjalista", Department = "Wydział Analiz Danych", JobPositionId = 8, EmployedSince = new DateOnly(2023, 4, 3) },
                new Employee { Id = 15, FirstName = "Bartosz", LastName = "Wróbel", Position = "Starszy specjalista", Department = "Wydział Analiz Danych", JobPositionId = 9, EmployedSince = new DateOnly(2022, 1, 17) },
                new Employee { Id = 16, FirstName = "Monika", LastName = "Sikora", Position = "Specjalista", Department = "Wydział Analiz Danych", JobPositionId = 10, EmployedSince = new DateOnly(2019, 11, 28) },
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
                new EmployeeSupervisor { EmployeeId = 16, SupervisorId = 3 },
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
                    Id = 1, EmployeeId = 1, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2022, 12, 11, 9, 0, 0),
                    PeriodStart = new DateOnly(2022, 12, 16), PeriodEnd = new DateOnly(2024, 12, 10), DueDate = new DateOnly(2024, 12, 15),
                    Expectations = "Samodzielne przygotowywanie analiz dla kierownictwa w uzgodnionych terminach, z opisem źródeł i ograniczeń danych.",
                    PartIConfirmedAt = new DateTime(2022, 12, 16, 15, 0, 0), EmployeeAcquaintedOn = new DateOnly(2022, 12, 16),
                    StartedAt = new DateTime(2024, 11, 20, 9, 0, 0), CompletedAt = new DateTime(2024, 12, 10, 14, 0, 0),
                    PreparedOn = new DateOnly(2024, 12, 10), PartIIAcquaintedOn = new DateOnly(2024, 12, 10),
                },
                new Evaluation
                {
                    Id = 2, EmployeeId = 1, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.InProgress, SheetConfirmedAt = new DateTime(2024, 12, 11, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 12, 16), DueDate = new DateOnly(2026, 12, 15),
                    Expectations = "Analizy dla kierownictwa oddawane w terminie i bez poprawek merytorycznych. Propozycje nowych wskaźników zgłaszane z własnej inicjatywy, co najmniej raz na kwartał.",
                    Goals = "Opracowanie zestawu wskaźników realizacji zadań departamentu do końca II kwartału 2026 r.",
                    PartIConfirmedAt = new DateTime(2024, 12, 16, 15, 0, 0), EmployeeAcquaintedOn = new DateOnly(2024, 12, 16),
                    StartedAt = new DateTime(2026, 11, 12, 10, 30, 0),
                },
                new Evaluation
                {
                    Id = 3, EmployeeId = 2, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2022, 11, 5, 9, 0, 0),
                    PeriodStart = new DateOnly(2022, 11, 7), PeriodEnd = new DateOnly(2024, 10, 28), DueDate = new DateOnly(2024, 11, 2),
                    Expectations = "Zestawienia danych przygotowywane według przyjętego wzoru i sprawdzone przed przekazaniem.",
                    PartIConfirmedAt = new DateTime(2022, 11, 7, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2022, 11, 7),
                    StartedAt = new DateTime(2024, 10, 6, 9, 0, 0), CompletedAt = new DateTime(2024, 10, 28, 12, 0, 0),
                    PreparedOn = new DateOnly(2024, 10, 28), PartIIAcquaintedOn = new DateOnly(2024, 10, 28),
                },
                new Evaluation
                {
                    Id = 4, EmployeeId = 9, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2023, 1, 14, 9, 0, 0),
                    PeriodStart = new DateOnly(2023, 1, 16), PeriodEnd = new DateOnly(2025, 1, 8), DueDate = new DateOnly(2025, 1, 10),
                    Expectations = "Korespondencja wydziału rejestrowana na bieżąco, odpowiedzi na proste pytania udzielane w ciągu dwóch dni.",
                    PartIConfirmedAt = new DateTime(2023, 1, 16, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2023, 1, 16),
                    StartedAt = new DateTime(2024, 12, 15, 9, 0, 0), CompletedAt = new DateTime(2025, 1, 8, 11, 0, 0),
                    PreparedOn = new DateOnly(2025, 1, 8), PartIIAcquaintedOn = new DateOnly(2025, 1, 8),
                },
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
                    Id = 6, EmployeeId = 9, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2025, 1, 13, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 1, 15), DueDate = new DateOnly(2027, 1, 10),
                    Expectations = "Rejestr korespondencji prowadzony bez zaległości. Zestawienia danych przygotowywane samodzielnie według wskazówek, bez konieczności poprawek.",
                    PartIConfirmedAt = new DateTime(2025, 1, 15, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 1, 15),
                    StartedAt = new DateTime(2026, 11, 16, 14, 0, 0),
                },
                new Evaluation
                {
                    Id = 7, EmployeeId = 12, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2022, 12, 23, 9, 0, 0),
                    PeriodStart = new DateOnly(2023, 1, 8), PeriodEnd = new DateOnly(2024, 12, 18), DueDate = new DateOnly(2024, 12, 20),
                    Expectations = "Cykliczne raporty przekazywane w terminie, z krótkim omówieniem najważniejszych zmian w danych.",
                    PartIConfirmedAt = new DateTime(2023, 1, 8, 14, 0, 0), EmployeeAcquaintedOn = new DateOnly(2023, 1, 8),
                    StartedAt = new DateTime(2024, 11, 24, 9, 0, 0), CompletedAt = new DateTime(2024, 12, 18, 10, 0, 0),
                    PreparedOn = new DateOnly(2024, 12, 18), PartIIAcquaintedOn = new DateOnly(2024, 12, 18),
                },
                new Evaluation
                {
                    Id = 8, EmployeeId = 13, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.NotStarted, SheetConfirmedAt = new DateTime(2024, 12, 12, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 12, 17), DueDate = new DateOnly(2026, 12, 10),
                    Expectations = "Analizy danych przygotowywane samodzielnie, z wnioskami zrozumiałymi dla odbiorców spoza wydziału.",
                    PartIConfirmedAt = new DateTime(2024, 12, 17, 15, 0, 0), EmployeeAcquaintedOn = new DateOnly(2024, 12, 17),
                },
                new Evaluation
                {
                    Id = 9, EmployeeId = 10, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.NotStarted, SheetConfirmedAt = new DateTime(2026, 11, 12, 9, 0, 0),
                    PeriodStart = new DateOnly(2026, 11, 6), DueDate = new DateOnly(2028, 11, 6),
                    Expectations = "Analizy wpływu zmian na usługi cyfrowe z rekomendacją i oceną ryzyka. Samodzielne przedstawianie stanowiska wydziału na posiedzeniach zespołów międzyresortowych.",
                },
                new Evaluation
                {
                    Id = 10, EmployeeId = 14, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2023, 4, 8, 9, 0, 0),
                    PeriodStart = new DateOnly(2023, 4, 22), PeriodEnd = new DateOnly(2025, 3, 30), DueDate = new DateOnly(2025, 4, 3),
                    Expectations = "Zestawy wskaźników przygotowywane z opisem metody pomiaru i źródeł danych, uzgodnione z komórkami merytorycznymi.",
                    PartIConfirmedAt = new DateTime(2023, 4, 22, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2023, 4, 22),
                    StartedAt = new DateTime(2025, 3, 2, 9, 0, 0), CompletedAt = new DateTime(2025, 3, 30, 12, 0, 0),
                    PreparedOn = new DateOnly(2025, 3, 30), PartIIAcquaintedOn = new DateOnly(2025, 3, 30),
                },
                new Evaluation
                {
                    Id = 11, EmployeeId = 14, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2025, 4, 7, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 4, 21), DueDate = new DateOnly(2027, 4, 3),
                    Expectations = "Analizy porównawcze dla kierownictwa przekazywane w uzgodnionych terminach. Koordynowanie prac analitycznych kilku osób wydziału bez opóźnień.",
                    Goals = "Opracowanie standardu opisu i weryfikacji źródeł danych dla wydziału do końca 2026 r.",
                    PartIConfirmedAt = new DateTime(2025, 4, 21, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 4, 21),
                },
                new Evaluation
                {
                    Id = 12, EmployeeId = 15, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2023, 1, 21, 9, 0, 0),
                    PeriodStart = new DateOnly(2023, 2, 4), PeriodEnd = new DateOnly(2025, 1, 14), DueDate = new DateOnly(2025, 1, 17),
                    Expectations = "Prognozy i analizy trendów przygotowywane w terminie, z opisem założeń i ograniczeń.",
                    PartIConfirmedAt = new DateTime(2023, 2, 4, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2023, 2, 4),
                    StartedAt = new DateTime(2024, 12, 1, 9, 0, 0), CompletedAt = new DateTime(2025, 1, 14, 12, 0, 0),
                    PreparedOn = new DateOnly(2025, 1, 14), PartIIAcquaintedOn = new DateOnly(2025, 1, 14),
                },
                new Evaluation
                {
                    Id = 13, EmployeeId = 15, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet, SheetConfirmedAt = new DateTime(2025, 1, 20, 9, 0, 0),
                    PeriodStart = new DateOnly(2025, 2, 3), DueDate = new DateOnly(2027, 1, 17),
                    Expectations = "Obliczenia w raportach wydziału sprawdzane przed wysłaniem. Wyjaśnienia metod analizy dla innych komórek udzielane w ciągu tygodnia.",
                    PartIConfirmedAt = new DateTime(2025, 2, 3, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2025, 2, 3),
                },
                new Evaluation
                {
                    Id = 14, EmployeeId = 16, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed, SheetConfirmedAt = new DateTime(2022, 12, 3, 9, 0, 0),
                    PeriodStart = new DateOnly(2022, 12, 17), PeriodEnd = new DateOnly(2024, 11, 28), DueDate = new DateOnly(2024, 11, 28),
                    Expectations = "Udostępnianie danych publicznych na wnioski w ustawowym terminie, z kompletnym uzasadnieniem odmowy.",
                    PartIConfirmedAt = new DateTime(2022, 12, 17, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2022, 12, 17),
                    WrittenEvaluationAt = new DateTime(2024, 11, 20, 13, 0, 0),
                    StartedAt = new DateTime(2024, 10, 1, 9, 0, 0), CompletedAt = new DateTime(2024, 11, 28, 12, 0, 0),
                    PreparedOn = new DateOnly(2024, 11, 28), PartIIAcquaintedOn = new DateOnly(2024, 11, 28),
                },
                new Evaluation
                {
                    Id = 15, EmployeeId = 16, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.InProgress, SheetConfirmedAt = new DateTime(2024, 12, 2, 9, 0, 0),
                    PeriodStart = new DateOnly(2024, 12, 16), DueDate = new DateOnly(2026, 11, 28),
                    Expectations = "Wnioski o udostępnienie danych rozpatrywane w terminie, także te wymagające uzgodnień z innymi komórkami. Rejestr udostępnionych danych prowadzony na bieżąco.",
                    PartIConfirmedAt = new DateTime(2024, 12, 16, 10, 0, 0), EmployeeAcquaintedOn = new DateOnly(2024, 12, 16),
                    WrittenEvaluationAt = new DateTime(2026, 11, 19, 14, 30, 0),
                    StartedAt = new DateTime(2026, 10, 1, 9, 0, 0),
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
                new Conversation { Id = 1, EvaluationId = 1, Type = ConversationType.Starting, At = new DateTime(2022, 12, 16, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 2, EvaluationId = 1, Type = ConversationType.Assessment, At = new DateTime(2024, 12, 8, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 3, EvaluationId = 2, Type = ConversationType.Starting, At = new DateTime(2024, 12, 16, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 4, EvaluationId = 2, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 27, 10, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 5, EvaluationId = 3, Type = ConversationType.Starting, At = new DateTime(2022, 11, 7, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 6, EvaluationId = 3, Type = ConversationType.Assessment, At = new DateTime(2024, 10, 27, 11, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 7, EvaluationId = 4, Type = ConversationType.Starting, At = new DateTime(2023, 1, 16, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 8, EvaluationId = 4, Type = ConversationType.Assessment, At = new DateTime(2025, 1, 7, 9, 30, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 9, EvaluationId = 5, Type = ConversationType.Starting, At = new DateTime(2024, 11, 17, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 10, EvaluationId = 5, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 5, 13, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 11, EvaluationId = 6, Type = ConversationType.Starting, At = new DateTime(2025, 1, 15, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 12, EvaluationId = 6, Type = ConversationType.Assessment, At = new DateTime(2026, 12, 18, 9, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 13, EvaluationId = 7, Type = ConversationType.Starting, At = new DateTime(2023, 1, 8, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 14, EvaluationId = 7, Type = ConversationType.Assessment, At = new DateTime(2024, 12, 17, 12, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 15, EvaluationId = 8, Type = ConversationType.Starting, At = new DateTime(2024, 12, 17, 11, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 16, EvaluationId = 8, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 14, 10, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 17, EvaluationId = 9, Type = ConversationType.Starting, At = new DateTime(2026, 11, 26, 10, 0, 0), Status = ConversationStatus.Scheduled },
                new Conversation { Id = 18, EvaluationId = 10, Type = ConversationType.Starting, At = new DateTime(2023, 4, 22, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 19, EvaluationId = 10, Type = ConversationType.Assessment, At = new DateTime(2025, 3, 25, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 20, EvaluationId = 11, Type = ConversationType.Starting, At = new DateTime(2025, 4, 21, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 21, EvaluationId = 12, Type = ConversationType.Starting, At = new DateTime(2023, 2, 4, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 22, EvaluationId = 12, Type = ConversationType.Assessment, At = new DateTime(2025, 1, 12, 11, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 23, EvaluationId = 13, Type = ConversationType.Starting, At = new DateTime(2025, 2, 3, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 24, EvaluationId = 14, Type = ConversationType.Starting, At = new DateTime(2022, 12, 17, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 25, EvaluationId = 14, Type = ConversationType.Assessment, At = new DateTime(2024, 11, 17, 10, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 26, EvaluationId = 15, Type = ConversationType.Starting, At = new DateTime(2024, 12, 16, 9, 0, 0), Status = ConversationStatus.Held },
                new Conversation { Id = 27, EvaluationId = 15, Type = ConversationType.Assessment, At = new DateTime(2026, 11, 16, 11, 0, 0), Status = ConversationStatus.Held },
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
                new EvaluationAdditionalCriterion { EvaluationId = 9, Criterion = Criterion.PublicSpeaking },
                new EvaluationAdditionalCriterion { EvaluationId = 10, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 11, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 12, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 13, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 14, Criterion = Criterion.AnalyticalThinking },
                new EvaluationAdditionalCriterion { EvaluationId = 15, Criterion = Criterion.AnalyticalThinking },
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
                new CriterionDecision { EvaluationId = 1, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 10, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 1, Criterion = Criterion.Reliability, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 10, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 1, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 10, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 1, Criterion = Criterion.Cooperation, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 10, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 1, Criterion = Criterion.Communication, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 10, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 1, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 10, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 3, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 10, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 3, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 10, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 3, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 10, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 3, Criterion = Criterion.Cooperation, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 10, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 3, Criterion = Criterion.Communication, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 10, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 3, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 10, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 4, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 1, 8, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 4, Criterion = Criterion.Reliability, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2025, 1, 8, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 4, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 1, 8, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 4, Criterion = Criterion.Cooperation, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 1, 8, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 4, Criterion = Criterion.Communication, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 1, 8, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 4, Criterion = Criterion.CustomerOrientation, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2025, 1, 8, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.WorkOrganization, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.Cooperation, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.Communication, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 5, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 5, Level = 5, DecidedAt = new DateTime(2026, 11, 6, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 7, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 18, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 7, Criterion = Criterion.Reliability, SuggestedLevel = 2, Level = 2, DecidedAt = new DateTime(2024, 12, 18, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 7, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 18, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 7, Criterion = Criterion.Cooperation, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 18, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 7, Criterion = Criterion.Communication, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 18, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 7, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 18, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 10, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2025, 3, 30, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 10, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 3, 30, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 10, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 3, 30, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 10, Criterion = Criterion.Cooperation, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2025, 3, 30, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 10, Criterion = Criterion.Communication, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 3, 30, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 10, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2025, 3, 30, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 12, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 1, 14, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 12, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 1, 14, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 12, Criterion = Criterion.WorkOrganization, SuggestedLevel = 2, Level = 2, DecidedAt = new DateTime(2025, 1, 14, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 12, Criterion = Criterion.Cooperation, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 1, 14, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 12, Criterion = Criterion.Communication, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2025, 1, 14, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 12, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2025, 1, 14, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 14, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 11, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 14, Criterion = Criterion.Reliability, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 11, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 14, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 11, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 14, Criterion = Criterion.Cooperation, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 11, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 14, Criterion = Criterion.Communication, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 11, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 14, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 11, 28, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 15, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 19, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 15, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 19, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 15, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 19, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 15, Criterion = Criterion.Cooperation, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 19, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 15, Criterion = Criterion.Communication, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2026, 11, 19, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 15, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2026, 11, 19, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.KnowledgeAndDevelopment, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.Reliability, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.WorkOrganization, SuggestedLevel = 3, Level = 3, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.Cooperation, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.Communication, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) },
                new CriterionDecision { EvaluationId = 16, Criterion = Criterion.AnalyticalThinking, SuggestedLevel = 4, Level = 4, DecidedAt = new DateTime(2024, 12, 5, 9, 0, 0) });
        });
    }

    private static string Lines(params string[] lines) => string.Join('\n', lines);
}
