using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSimulation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityAndCalendars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Transaction",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateOnly>(
                name: "End",
                table: "CalendarEvents",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2023, 4, 29));

            migrationBuilder.AddColumn<DateOnly>(
                name: "Start",
                table: "CalendarEvents",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2023, 4, 29));

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountRoles",
                schema: "Security",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => new { x.AccountId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AccountRoles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountRoles_Roles_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Security",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Security",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRoles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Security");

            migrationBuilder.DropColumn(
                name: "End",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "CalendarEvents");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Date",
                table: "Transaction",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
