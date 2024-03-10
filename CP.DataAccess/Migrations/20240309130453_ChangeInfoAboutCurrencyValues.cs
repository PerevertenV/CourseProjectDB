using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInfoAboutCurrencyValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AvailOfAskedCourse",
                table: "InfoAboutCurrency",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "InfoAboutCurrency",
                keyColumn: "ID",
                keyValue: 1,
                column: "AvailOfAskedCourse",
                value: 5000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AvailOfAskedCourse",
                table: "InfoAboutCurrency",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "InfoAboutCurrency",
                keyColumn: "ID",
                keyValue: 1,
                column: "AvailOfAskedCourse",
                value: 5000.0);
        }
    }
}
