using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinanceManagement.Migrations
{
    public partial class splitsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_splits_transactions_transactionid",
                table: "splits");

            migrationBuilder.DropForeignKey(
                name: "FK_splits_transactions_transactionId",
                table: "splits");

            migrationBuilder.DropIndex(
                name: "IX_splits_transactionid",
                table: "splits");

            migrationBuilder.DropIndex(
                name: "IX_splits_transactionId",
                table: "splits");

            migrationBuilder.DropColumn(
                name: "transactionId",
                table: "splits");

            migrationBuilder.DropColumn(
                name: "transactionid",
                table: "splits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "transactionId",
                table: "splits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "transactionid",
                table: "splits",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_splits_transactionid",
                table: "splits",
                column: "transactionid");

            migrationBuilder.CreateIndex(
                name: "IX_splits_transactionId",
                table: "splits",
                column: "transactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_splits_transactions_transactionid",
                table: "splits",
                column: "transactionid",
                principalTable: "transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_splits_transactions_transactionId",
                table: "splits",
                column: "transactionId",
                principalTable: "transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
