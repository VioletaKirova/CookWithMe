using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CookWithMe.Data.Migrations
{
    public partial class AddingAddedOnFieldToMappingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "UserShoppingLists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "UserFavoriteRecipes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "UserCookedRecipes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "UserShoppingLists");

            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "UserFavoriteRecipes");

            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "UserCookedRecipes");
        }
    }
}
