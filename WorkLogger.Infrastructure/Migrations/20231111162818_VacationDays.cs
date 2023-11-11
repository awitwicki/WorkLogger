using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkLogger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VacationDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VacationDaysPerYear",
                table: "EmployeeSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VacationDaysPerYear",
                table: "EmployeeSettings");
        }
    }
}
