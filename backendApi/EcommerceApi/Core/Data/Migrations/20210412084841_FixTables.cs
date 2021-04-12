using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceApi.Core.Data.Migrations
{
    public partial class FixTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "LangCode",
                table: "ProductCategories");

            migrationBuilder.AddColumn<int>(
                name: "SeqNo",
                table: "ProductCategories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeqNo",
                table: "ProductCategories");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "ProductCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LangCode",
                table: "ProductCategories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
