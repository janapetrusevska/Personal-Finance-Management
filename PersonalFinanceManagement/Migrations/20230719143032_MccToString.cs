using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinanceManagement.Migrations
{
    public partial class MccToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MccCode",
                table: "transactions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "transactions",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Kind",
                table: "transactions",
                newName: "kind");

            migrationBuilder.RenameColumn(
                name: "Direction",
                table: "transactions",
                newName: "direction");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "transactions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "transactions",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "transactions",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "CatCode",
                table: "transactions",
                newName: "catCode");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "transactions",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transactions",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "mcc",
                table: "transactions",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mcc",
                table: "transactions");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "transactions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "kind",
                table: "transactions",
                newName: "Kind");

            migrationBuilder.RenameColumn(
                name: "direction",
                table: "transactions",
                newName: "Direction");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "transactions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "transactions",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "transactions",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "catCode",
                table: "transactions",
                newName: "CatCode");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "transactions",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "transactions",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "MccCode",
                table: "transactions",
                type: "character varying(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");
        }
    }
}
