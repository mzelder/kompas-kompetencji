using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Text;

namespace KompasKompetencji.Web.Organization;

public static class EmployeeSearch
{
    public static IReadOnlyList<Employee> Filter(IReadOnlyList<Employee> employees, string? term)
    {
        var normalizedTerm = TextNormalizer.Normalize(term?.Trim() ?? string.Empty);
        if (normalizedTerm.Length == 0)
        {
            return employees;
        }

        return employees.Where(e => Matches(e, normalizedTerm)).ToList();
    }

    private static bool Matches(Employee employee, string normalizedTerm) =>
        TextNormalizer.Normalize(employee.FullName).Contains(normalizedTerm)
        || TextNormalizer.Normalize(employee.Position).Contains(normalizedTerm)
        || TextNormalizer.Normalize(employee.Department).Contains(normalizedTerm);
}
