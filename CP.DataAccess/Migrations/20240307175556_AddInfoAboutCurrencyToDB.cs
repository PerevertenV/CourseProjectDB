using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInfoAboutCurrencyToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoAboutCurrency",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AskedCoursePriceTo = table.Column<double>(type: "float", nullable: false),
                    AvailOfAskedCourse = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoAboutCurrency", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "InfoAboutCurrency",
                columns: new[] { "ID", "AskedCoursePriceTo", "AvailOfAskedCourse", "Name" },
                values: new object[] { 1, 37.0, 5000.0, "USD" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoAboutCurrency");
        }
    }
}
