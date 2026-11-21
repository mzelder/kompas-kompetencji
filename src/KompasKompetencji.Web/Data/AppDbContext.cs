using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<EmployeeSupervisor> EmployeeSupervisors => Set<EmployeeSupervisor>();

    public DbSet<Evaluation> Evaluations => Set<Evaluation>();

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
                    Id = 1, EmployeeId = 1, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed,
                    PeriodStart = new DateOnly(2024, 12, 15), PeriodEnd = new DateOnly(2025, 12, 14), DueDate = new DateOnly(2025, 12, 15),
                    ConversationAt = new DateTime(2025, 12, 8, 10, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 11, 20, 9, 0, 0), CompletedAt = new DateTime(2025, 12, 10, 14, 0, 0),
                },
                new Evaluation
                {
                    Id = 2, EmployeeId = 1, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.InProgress,
                    PeriodStart = new DateOnly(2025, 12, 15), PeriodEnd = new DateOnly(2026, 12, 14), DueDate = new DateOnly(2026, 12, 15),
                    ConversationAt = new DateTime(2026, 11, 27, 10, 0, 0), ConversationStatus = ConversationStatus.Scheduled,
                    StartedAt = new DateTime(2026, 11, 12, 10, 30, 0),
                },
                new Evaluation
                {
                    Id = 3, EmployeeId = 2, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed,
                    PeriodStart = new DateOnly(2024, 11, 2), PeriodEnd = new DateOnly(2025, 11, 1), DueDate = new DateOnly(2025, 11, 2),
                    ConversationAt = new DateTime(2025, 10, 27, 11, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 10, 6, 9, 0, 0), CompletedAt = new DateTime(2025, 10, 28, 12, 0, 0),
                },
                new Evaluation
                {
                    Id = 4, EmployeeId = 9, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed,
                    PeriodStart = new DateOnly(2025, 1, 10), PeriodEnd = new DateOnly(2026, 1, 9), DueDate = new DateOnly(2026, 1, 10),
                    ConversationAt = new DateTime(2026, 1, 7, 9, 30, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 12, 15, 9, 0, 0), CompletedAt = new DateTime(2026, 1, 8, 11, 0, 0),
                },
                new Evaluation
                {
                    Id = 5, EmployeeId = 10, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed,
                    PeriodStart = new DateOnly(2025, 6, 1), PeriodEnd = new DateOnly(2026, 5, 31), DueDate = new DateOnly(2026, 6, 1),
                    ConversationAt = new DateTime(2026, 5, 26, 13, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2026, 5, 4, 9, 0, 0), CompletedAt = new DateTime(2026, 5, 27, 13, 0, 0),
                },
                new Evaluation
                {
                    Id = 6, EmployeeId = 9, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.CriteriaSet,
                    PeriodStart = new DateOnly(2026, 1, 10), PeriodEnd = new DateOnly(2027, 1, 9), DueDate = new DateOnly(2027, 1, 10),
                    ConversationAt = new DateTime(2026, 12, 18, 9, 0, 0), ConversationStatus = ConversationStatus.Scheduled,
                    StartedAt = new DateTime(2026, 11, 16, 14, 0, 0),
                },
                new Evaluation
                {
                    Id = 7, EmployeeId = 12, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.Completed,
                    PeriodStart = new DateOnly(2024, 12, 20), PeriodEnd = new DateOnly(2025, 12, 19), DueDate = new DateOnly(2025, 12, 20),
                    ConversationAt = new DateTime(2025, 12, 17, 12, 0, 0), ConversationStatus = ConversationStatus.Held,
                    StartedAt = new DateTime(2025, 11, 24, 9, 0, 0), CompletedAt = new DateTime(2025, 12, 18, 10, 0, 0),
                },
                new Evaluation
                {
                    Id = 8, EmployeeId = 13, EvaluatorId = 3, Sheet = EvaluationSheet.C, Status = EvaluationStatus.NotStarted,
                    PeriodStart = new DateOnly(2025, 12, 10), PeriodEnd = new DateOnly(2026, 12, 9), DueDate = new DateOnly(2026, 12, 10),
                    ConversationAt = new DateTime(2026, 11, 14, 10, 0, 0), ConversationStatus = ConversationStatus.Scheduled,
                });
        });
    }
}
