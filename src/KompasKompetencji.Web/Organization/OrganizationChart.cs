using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Organization;

public sealed class OrganizationChart
{
    private readonly Dictionary<int, Employee> employeesById;
    private readonly ILookup<int, int> supervisorIdsByEmployee;
    private readonly ILookup<int, int> subordinateIdsBySupervisor;
    private readonly IReadOnlyDictionary<int, string> accountLoginsByEmployee;

    public OrganizationChart(
        IEnumerable<Employee> employees,
        IEnumerable<EmployeeSupervisor> links,
        IReadOnlyDictionary<int, string> accountLoginsByEmployee)
    {
        Employees = SortByName(employees);
        Supervisors = Employees.Where(e => e.IsSupervisor).ToList();
        employeesById = Employees.ToDictionary(e => e.Id);

        var linkList = links.ToList();
        supervisorIdsByEmployee = linkList.ToLookup(l => l.EmployeeId, l => l.SupervisorId);
        subordinateIdsBySupervisor = linkList.ToLookup(l => l.SupervisorId, l => l.EmployeeId);
        this.accountLoginsByEmployee = accountLoginsByEmployee;
    }

    public static OrganizationChart Empty { get; } = new([], [], new Dictionary<int, string>());

    public IReadOnlyList<Employee> Employees { get; }

    public IReadOnlyList<Employee> Supervisors { get; }

    public Employee? Find(int employeeId) => employeesById.GetValueOrDefault(employeeId);

    public IReadOnlyList<Employee> SupervisorsOf(int employeeId) => Resolve(supervisorIdsByEmployee[employeeId]);

    public IReadOnlyList<Employee> SubordinatesOf(int supervisorId) => Resolve(subordinateIdsBySupervisor[supervisorId]);

    public string? AccountLoginOf(int employeeId) => accountLoginsByEmployee.GetValueOrDefault(employeeId);

    public IReadOnlyList<Employee> AssignableSupervisorsFor(int employeeId)
    {
        var excluded = AllSubordinatesOf(employeeId);
        excluded.Add(employeeId);
        excluded.UnionWith(supervisorIdsByEmployee[employeeId]);
        return Supervisors.Where(s => !excluded.Contains(s.Id)).ToList();
    }

    private HashSet<int> AllSubordinatesOf(int supervisorId)
    {
        var found = new HashSet<int>();
        var pending = new Queue<int>([supervisorId]);
        while (pending.TryDequeue(out var current))
        {
            foreach (var subordinateId in subordinateIdsBySupervisor[current])
            {
                if (found.Add(subordinateId))
                {
                    pending.Enqueue(subordinateId);
                }
            }
        }
        return found;
    }

    private IReadOnlyList<Employee> Resolve(IEnumerable<int> employeeIds) =>
        SortByName(employeeIds.Select(Find).OfType<Employee>());

    private static List<Employee> SortByName(IEnumerable<Employee> employees) =>
        employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
}
