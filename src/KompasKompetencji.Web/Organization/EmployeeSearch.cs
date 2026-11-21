using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Organization;

public static class EmployeeSearch
{
    private static readonly Dictionary<char, char> PolishLetters = new()
    {
        ['ą'] = 'a', ['ć'] = 'c', ['ę'] = 'e', ['ł'] = 'l', ['ń'] = 'n',
        ['ó'] = 'o', ['ś'] = 's', ['ź'] = 'z', ['ż'] = 'z',
    };

    public static IReadOnlyList<Employee> Filter(IReadOnlyList<Employee> employees, string? term)
    {
        var normalizedTerm = Normalize(term?.Trim() ?? string.Empty);
        if (normalizedTerm.Length == 0)
        {
            return employees;
        }

        return employees.Where(e => Matches(e, normalizedTerm)).ToList();
    }

    private static bool Matches(Employee employee, string normalizedTerm) =>
        Normalize(employee.FullName).Contains(normalizedTerm)
        || Normalize(employee.Position).Contains(normalizedTerm)
        || Normalize(employee.Department).Contains(normalizedTerm);

    private static string Normalize(string text) =>
        string.Concat(text.ToLowerInvariant().Select(c => PolishLetters.GetValueOrDefault(c, c)));
}
