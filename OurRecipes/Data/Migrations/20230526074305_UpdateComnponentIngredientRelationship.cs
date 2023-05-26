using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurRecipes.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComnponentIngredientRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Ingredients_IngredientId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_IngredientId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ComponentId",
                table: "Ingredients");

            migrationBuilder.CreateIndex(
                name: "IX_Components_IngredientId",
                table: "Components",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Ingredients_IngredientId",
                table: "Components",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Ingredients_IngredientId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_IngredientId",
                table: "Components");

            migrationBuilder.AddColumn<int>(
                name: "ComponentId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Components_IngredientId",
                table: "Components",
                column: "IngredientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Ingredients_IngredientId",
                table: "Components",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id");
        }
    }
}
