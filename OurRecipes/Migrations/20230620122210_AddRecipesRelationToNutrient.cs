using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurRecipes.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipesRelationToNutrient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nutrients_Recipes_RecipeId",
                table: "Nutrients");

            migrationBuilder.DropIndex(
                name: "IX_Nutrients_RecipeId",
                table: "Nutrients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Nutrients");

            migrationBuilder.CreateTable(
                name: "NutrientRecipe",
                columns: table => new
                {
                    NutrientsId = table.Column<int>(type: "int", nullable: false),
                    RecipesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientRecipe", x => new { x.NutrientsId, x.RecipesId });
                    table.ForeignKey(
                        name: "FK_NutrientRecipe_Nutrients_NutrientsId",
                        column: x => x.NutrientsId,
                        principalTable: "Nutrients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NutrientRecipe_Recipes_RecipesId",
                        column: x => x.RecipesId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NutrientRecipe_RecipesId",
                table: "NutrientRecipe",
                column: "RecipesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NutrientRecipe");

            migrationBuilder.AddColumn<string>(
                name: "RecipeId",
                table: "Nutrients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_RecipeId",
                table: "Nutrients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrients_Recipes_RecipeId",
                table: "Nutrients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
