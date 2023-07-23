using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinanceManagement.Migrations
{
    public partial class categoryForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "categorycode",
                table: "transactions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_catCode",
                table: "transactions",
                column: "catCode");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_categorycode",
                table: "transactions",
                column: "categorycode");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_catCode",
                table: "transactions",
                column: "catCode",
                principalTable: "categories",
                principalColumn: "code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_categorycode",
                table: "transactions",
                column: "categorycode",
                principalTable: "categories",
                principalColumn: "code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_catCode",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_categorycode",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_catCode",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_categorycode",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "categorycode",
                table: "transactions");
        }
    }
}
