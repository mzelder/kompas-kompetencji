using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121135935_AddOrganizationStructure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "EmployeeId",
            table: "Users",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Department",
            table: "Employees",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<bool>(
            name: "IsSupervisor",
            table: "Employees",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateTable(
            name: "EmployeeSupervisors",
            columns: table => new
            {
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                SupervisorId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmployeeSupervisors", x => new { x.EmployeeId, x.SupervisorId });
                table.CheckConstraint("CK_EmployeeSupervisors_NotSelf", "[EmployeeId] <> [SupervisorId]");
                table.ForeignKey(
                    name: "FK_EmployeeSupervisors_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_EmployeeSupervisors_Employees_SupervisorId",
                    column: x => x.SupervisorId,
                    principalTable: "Employees",
                    principalColumn: "Id");
            });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[,]
            {
                { 1, 3 },
                { 2, 3 }
            });

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 1,
            columns: new[] { "Department", "IsSupervisor" },
            values: new object[] { "Wydział Analiz Danych", false });

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 2,
            columns: new[] { "Department", "IsSupervisor" },
            values: new object[] { "Wydział Analiz Danych", false });

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 3,
            columns: new[] { "Department", "IsSupervisor" },
            values: new object[] { "Wydział Analiz Danych", true });

        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "FirstName", "IsSupervisor", "LastName", "Position" },
            values: new object[,]
            {
                { 4, "Departament Danych i Usług Cyfrowych", "Tomasz", true, "Zieliński", "Dyrektor departamentu" },
                { 5, "Wydział Usług Cyfrowych", "Marek", true, "Lewandowski", "Naczelnik wydziału" },
                { 6, "Wydział Usług Cyfrowych", "Magdalena", true, "Dąbrowska", "Zastępca naczelnika wydziału" },
                { 7, "Wydział Usług Cyfrowych", "Katarzyna", false, "Wójcik", "Główny specjalista" },
                { 8, "Wydział Usług Cyfrowych", "Piotr", false, "Kamiński", "Starszy referent" }
            });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[,]
            {
                { 3, 4 },
                { 5, 4 },
                { 6, 5 },
                { 7, 5 },
                { 8, 5 },
                { 8, 6 }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Users_EmployeeId",
            table: "Users",
            column: "EmployeeId",
            unique: true,
            filter: "[EmployeeId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeSupervisors_SupervisorId",
            table: "EmployeeSupervisors",
            column: "SupervisorId");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Employees_EmployeeId",
            table: "Users",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Employees_EmployeeId",
            table: "Users");

        migrationBuilder.DropTable(
            name: "EmployeeSupervisors");

        migrationBuilder.DropIndex(
            name: "IX_Users_EmployeeId",
            table: "Users");

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DropColumn(
            name: "EmployeeId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "Department",
            table: "Employees");

        migrationBuilder.DropColumn(
            name: "IsSupervisor",
            table: "Employees");
    }
}
