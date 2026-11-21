namespace KompasKompetencji.Web.Data;

public class Employee
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Position { get; set; }

    public required string Department { get; set; }

    public bool IsSupervisor { get; set; }

    public DateOnly EmployedSince { get; set; }

    public ICollection<EmployeeSupervisor> SupervisorLinks { get; set; } = [];

    public ICollection<EmployeeSupervisor> SubordinateLinks { get; set; } = [];

    public JobDescription? JobDescription { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
