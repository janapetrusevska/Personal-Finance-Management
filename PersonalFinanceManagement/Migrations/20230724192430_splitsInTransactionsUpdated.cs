using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinanceManagement.Migrations
{
    public partial class splitsInTransactionsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_splits_transactions_catCode",
                table: "splits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_splits",
                table: "splits");

            migrationBuilder.DropIndex(
                name: "IX_splits_catCode",
                table: "splits");

            migrationBuilder.DropColumn(
                name: "id",
                table: "splits");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "id",
                table: "splits",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_splits",
                table: "splits",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_splits_catCode",
                table: "splits",
                column: "catCode");

            migrationBuilder.AddForeignKey(
                name: "FK_splits_transactions_catCode",
                table: "splits",
                column: "catCode",
                principalTable: "transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
