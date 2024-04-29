using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSimulation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecureCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RoleId",
                table: "Categories",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Roles_RoleId",
                table: "Categories",
                column: "RoleId",
                principalSchema: "Security",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Roles_RoleId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_RoleId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Categories");
        }
    }
}
