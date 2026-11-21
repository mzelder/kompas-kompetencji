using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<EmployeeSupervisor> EmployeeSupervisors => Set<EmployeeSupervisor>();

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
                new Employee { Id = 1, FirstName = "Anna", LastName = "Kowalska", Position = "Starszy specjalista", Department = "Wydział Analiz Danych" },
                new Employee { Id = 2, FirstName = "Jan", LastName = "Nowak", Position = "Specjalista", Department = "Wydział Analiz Danych" },
                new Employee { Id = 3, FirstName = "Ewa", LastName = "Wiśniewska", Position = "Naczelnik wydziału", Department = "Wydział Analiz Danych", IsSupervisor = true },
                new Employee { Id = 4, FirstName = "Tomasz", LastName = "Zieliński", Position = "Dyrektor departamentu", Department = "Departament Danych i Usług Cyfrowych", IsSupervisor = true },
                new Employee { Id = 5, FirstName = "Marek", LastName = "Lewandowski", Position = "Naczelnik wydziału", Department = "Wydział Usług Cyfrowych", IsSupervisor = true },
                new Employee { Id = 6, FirstName = "Magdalena", LastName = "Dąbrowska", Position = "Zastępca naczelnika wydziału", Department = "Wydział Usług Cyfrowych", IsSupervisor = true },
                new Employee { Id = 7, FirstName = "Katarzyna", LastName = "Wójcik", Position = "Główny specjalista", Department = "Wydział Usług Cyfrowych" },
                new Employee { Id = 8, FirstName = "Piotr", LastName = "Kamiński", Position = "Starszy referent", Department = "Wydział Usług Cyfrowych" });
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
                new EmployeeSupervisor { EmployeeId = 8, SupervisorId = 5 });
        });
    }
}
