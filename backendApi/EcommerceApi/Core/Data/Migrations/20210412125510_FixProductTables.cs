using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceApi.Core.Data.Migrations
{
    public partial class FixProductTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductAttributes_ProductAttributeValues_ProductAttributeValueId",
                table: "Product_ProductAttributes");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductAttributes_ProductAttributeValueId",
                table: "Product_ProductAttributes");

            migrationBuilder.DropColumn(
                name: "ProductAttributeValueId",
                table: "Product_ProductAttributes");

            migrationBuilder.AddColumn<int>(
                name: "ProductAttributeId",
                table: "ProductAttributeValues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_ProductAttributeId",
                table: "ProductAttributeValues",
                column: "ProductAttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_ProductAttributes_ProductAttributeId",
                table: "ProductAttributeValues",
                column: "ProductAttributeId",
                principalTable: "ProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_ProductAttributes_ProductAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeValues_ProductAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "ProductAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.AddColumn<int>(
                name: "ProductAttributeValueId",
                table: "Product_ProductAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductAttributes_ProductAttributeValueId",
                table: "Product_ProductAttributes",
                column: "ProductAttributeValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductAttributes_ProductAttributeValues_ProductAttributeValueId",
                table: "Product_ProductAttributes",
                column: "ProductAttributeValueId",
                principalTable: "ProductAttributeValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
