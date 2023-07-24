using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinanceManagement.Migrations
{
    public partial class splitsInTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
