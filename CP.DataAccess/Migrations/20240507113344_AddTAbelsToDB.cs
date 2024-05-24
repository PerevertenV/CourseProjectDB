using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTAbelsToDB : Migration
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
                name: "Payments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sum = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfMakingPayments = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.ID);
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
                    IDOfUser = table.Column<int>(type: "int", nullable: true),
                    IDOfEmployee = table.Column<int>(type: "int", nullable: true),
                    DepositedMoney = table.Column<double>(type: "float", nullable: true),
                    SumOfCurrency = table.Column<double>(type: "float", nullable: false),
                    MoneyToReturn = table.Column<double>(type: "float", nullable: true),
                    SumInUAH = table.Column<double>(type: "float", nullable: false),
                    DateOfMakingPurchase = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                        name: "FK_Purchase_Users_IDOfEmployee",
                        column: x => x.IDOfEmployee,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Purchase_Users_IDOfUser",
                        column: x => x.IDOfUser,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                table: "InfoAboutCurrency",
                columns: new[] { "ID", "AskedCoursePriceTo", "AvailOfAskedCourse", "Name" },
                values: new object[] { 1, 37.0, 5000, "USD" });

            migrationBuilder.InsertData(
                table: "Purchase",
                columns: new[] { "ID", "CurrencyID", "DateOfMakingPurchase", "DepositedMoney", "IDOfEmployee", "IDOfUser", "MoneyToReturn", "State", "SumInUAH", "SumOfCurrency" },
                values: new object[] { 1, 1, new DateTime(2024, 5, 7, 14, 33, 44, 289, DateTimeKind.Local).AddTicks(6911), 0.0, null, null, 0.0, "Заверешено", 0.0, 0.0 });

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CurrencyID",
                table: "Purchase",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_IDOfEmployee",
                table: "Purchase",
                column: "IDOfEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_IDOfUser",
                table: "Purchase",
                column: "IDOfUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "InfoAboutCurrency");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
