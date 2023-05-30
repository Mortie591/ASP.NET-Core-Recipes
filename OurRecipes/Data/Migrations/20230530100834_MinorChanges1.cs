using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurRecipes.Data.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Recipes_RecipeId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_RecipeId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Sections");

            migrationBuilder.CreateTable(
                name: "RecipeSection",
                columns: table => new
                {
                    RecipesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SectionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSection", x => new { x.RecipesId, x.SectionsId });
                    table.ForeignKey(
                        name: "FK_RecipeSection_Recipes_RecipesId",
                        column: x => x.RecipesId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeSection_Sections_SectionsId",
                        column: x => x.SectionsId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSection_SectionsId",
                table: "RecipeSection",
                column: "SectionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeSection");

            migrationBuilder.AddColumn<string>(
                name: "RecipeId",
                table: "Sections",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_RecipeId",
                table: "Sections",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Recipes_RecipeId",
                table: "Sections",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
