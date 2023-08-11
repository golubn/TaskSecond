using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_B1_Task2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreated1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassName = table.Column<string>(type: "STRING", nullable: false),
                    FileName = table.Column<string>(type: "STRING", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassID);
                });

            migrationBuilder.CreateTable(
                name: "TotalSum",
                columns: table => new
                {
                    TotalSumID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InActSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    InPassiveSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    OutActSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    OutPassiveSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    TurnCredit = table.Column<decimal>(type: "TEXT", nullable: false),
                    TurnDebit = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalSum", x => x.TotalSumID);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataLoad = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    FileName = table.Column<string>(type: "STRING", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.FileID);
                });

            migrationBuilder.CreateTable(
                name: "AccountBalance",
                columns: table => new
                {
                    BalanceID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBalance", x => x.BalanceID);
                    table.ForeignKey(
                        name: "FK_AccountBalance_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalSumInClass",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalSumInClass", x => x.ClassID);
                    table.ForeignKey(
                        name: "FK_TotalSumInClass_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ClassID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    OutPassiveSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    BalanceID = table.Column<int>(type: "INTEGER", nullable: false),
                    InActSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    InPassiveSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    OutActSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    TurnCredit = table.Column<decimal>(type: "TEXT", nullable: false),
                    TurnDebit = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.OutPassiveSaldo);
                    table.ForeignKey(
                        name: "FK_Balance_AccountBalance_BalanceID",
                        column: x => x.BalanceID,
                        principalTable: "AccountBalance",
                        principalColumn: "BalanceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SumForPart",
                columns: table => new
                {
                    BalanceID = table.Column<int>(type: "INTEGER", nullable: false),
                    InActSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    InPassiveSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    TurnDebit = table.Column<decimal>(type: "TEXT", nullable: false),
                    TurnCredit = table.Column<decimal>(type: "TEXT", nullable: false),
                    OutActSaldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    OutPassiveSaldo = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SumForPart", x => x.BalanceID);
                    table.ForeignKey(
                        name: "FK_SumForPart_AccountBalance_BalanceID",
                        column: x => x.BalanceID,
                        principalTable: "AccountBalance",
                        principalColumn: "BalanceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountBalance_ClassId",
                table: "AccountBalance",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_BalanceID",
                table: "Balance",
                column: "BalanceID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balance");

            migrationBuilder.DropTable(
                name: "SumForPart");

            migrationBuilder.DropTable(
                name: "TotalSum");

            migrationBuilder.DropTable(
                name: "TotalSumInClass");

            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.DropTable(
                name: "AccountBalance");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
