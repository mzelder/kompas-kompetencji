namespace KompasKompetencji.Web.Data;

public class AppUser
{
    public int Id { get; set; }

    public required string Login { get; set; }

    public required string PasswordHash { get; set; }

    public required string Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? EmployeeId { get; set; }

    public Employee? Employee { get; set; }
}
