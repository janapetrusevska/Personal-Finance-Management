using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinanceManagement.Migrations
{
    public partial class RelationBetweenTransactionAndSplitEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "transactionId",
                table: "splits",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_splits_transactionId",
                table: "splits",
                column: "transactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_splits_transactions_transactionId",
                table: "splits",
                column: "transactionId",
                principalTable: "transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_splits_transactions_transactionId",
                table: "splits");

            migrationBuilder.DropIndex(
                name: "IX_splits_transactionId",
                table: "splits");

            migrationBuilder.DropColumn(
                name: "transactionId",
                table: "splits");
        }
    }
}
