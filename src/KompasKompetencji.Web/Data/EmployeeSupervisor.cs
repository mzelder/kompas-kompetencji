namespace KompasKompetencji.Web.Data;

public class EmployeeSupervisor
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public int SupervisorId { get; set; }

    public Employee Supervisor { get; set; } = null!;
}
