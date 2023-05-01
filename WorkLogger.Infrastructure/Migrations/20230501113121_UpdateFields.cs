using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkLogger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthWorkDays_AspNetUsers_UserId",
                table: "MonthWorkDays");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MonthWorkDays",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_MonthWorkDays_UserId",
                table: "MonthWorkDays",
                newName: "IX_MonthWorkDays_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthWorkDays_AspNetUsers_EmployeeId",
                table: "MonthWorkDays",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthWorkDays_AspNetUsers_EmployeeId",
                table: "MonthWorkDays");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "MonthWorkDays",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MonthWorkDays_EmployeeId",
                table: "MonthWorkDays",
                newName: "IX_MonthWorkDays_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthWorkDays_AspNetUsers_UserId",
                table: "MonthWorkDays",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
