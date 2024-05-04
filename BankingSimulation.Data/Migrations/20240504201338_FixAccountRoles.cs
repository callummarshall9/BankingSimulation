using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSimulation.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixAccountRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRoles_Roles_AccountId",
                schema: "Security",
                table: "AccountRoles");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Calendars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_RoleId",
                table: "Calendars",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_CalendarId",
                table: "CalendarEvents",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_RoleId",
                schema: "Security",
                table: "AccountRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Roles_RoleId",
                schema: "Security",
                table: "AccountRoles",
                column: "RoleId",
                principalSchema: "Security",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_Calendars_CalendarId",
                table: "CalendarEvents",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_Roles_RoleId",
                table: "Calendars",
                column: "RoleId",
                principalSchema: "Security",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRoles_Roles_RoleId",
                schema: "Security",
                table: "AccountRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_Calendars_CalendarId",
                table: "CalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_Roles_RoleId",
                table: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_RoleId",
                table: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_CalendarId",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_AccountRoles_RoleId",
                schema: "Security",
                table: "AccountRoles");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Calendars");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Roles_AccountId",
                schema: "Security",
                table: "AccountRoles",
                column: "AccountId",
                principalSchema: "Security",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
