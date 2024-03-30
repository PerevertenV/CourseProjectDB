using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTabelsToDB : Migration
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
                    AvailOfAskedCourse = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoAboutCurrency", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyID = table.Column<int>(type: "int", nullable: false),
                    IDOfUser = table.Column<int>(type: "int", nullable: false),
                    DepositedMoney = table.Column<double>(type: "float", nullable: false),
                    SumOfCurrency = table.Column<double>(type: "float", nullable: false),
                    MoneyToReturn = table.Column<double>(type: "float", nullable: false),
                    SumInUAH = table.Column<double>(type: "float", nullable: false),
                    DateOfMakingPurchase = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Purchase_InfoAboutCurrency_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "InfoAboutCurrency",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchase_Users_IDOfUser",
                        column: x => x.IDOfUser,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "InfoAboutCurrency",
                columns: new[] { "ID", "AskedCoursePriceTo", "AvailOfAskedCourse", "Name" },
                values: new object[] { 1, 37.0, 5000, "USD" });

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CurrencyID",
                table: "Purchase",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_IDOfUser",
                table: "Purchase",
                column: "IDOfUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "InfoAboutCurrency");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
