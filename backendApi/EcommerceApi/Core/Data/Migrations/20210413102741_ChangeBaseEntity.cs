using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceApi.Core.Data.Migrations
{
    public partial class ChangeBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Product_ProductAttributes",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "Product_ProductAttributes",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Product_Pictures",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "Product_Pictures",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Pictures",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "OrderAddresses",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "OrderAddresses",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Localized",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "Localized",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Language",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "Language",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "DeliveryMethods",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "DeliveryMethods",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Product_ProductAttributes");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Product_ProductAttributes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Product_Pictures");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Product_Pictures");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "OrderAddresses");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "OrderAddresses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Localized");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Localized");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "DeliveryMethods");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "DeliveryMethods");
        }
    }
}
