using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSimulation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceSystemId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceSystemId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceSystemId",
                table: "Transactions",
                column: "SourceSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Systems_SourceSystemId",
                table: "Transactions",
                column: "SourceSystemId",
                principalTable: "Systems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Systems_SourceSystemId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SourceSystemId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceSystemId",
                table: "Transactions");
        }
    }
}
