using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeThanhPhuongMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)1,
                column: "AccountRole",
                value: 3);

            migrationBuilder.InsertData(
                table: "SystemAccount",
                columns: new[] { "AccountID", "AccountEmail", "AccountName", "AccountPassword", "AccountRole" },
                values: new object[,]
                {
                    { (short)2, "staff@FUNewsManagementSystem.org", "Staff User", "staff123", 1 },
                    { (short)3, "lecturer@FUNewsManagementSystem.org", "Lecturer User", "lecturer123", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)3);

            migrationBuilder.UpdateData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)1,
                column: "AccountRole",
                value: 1);
        }
    }
}
