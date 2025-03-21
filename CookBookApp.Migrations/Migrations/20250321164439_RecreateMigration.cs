using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookBookApp.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RecreateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cuisine",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DishType",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainIngredient",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occasion",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cuisine",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "DishType",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "MainIngredient",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Occasion",
                table: "Recipes");
        }
    }
}
