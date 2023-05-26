using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurRecipes.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserFavouriteRecipeUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_AppIdentityUserId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_AppIdentityUserId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "AppIdentityUserId",
                table: "Recipes");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppIdentityUserId",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_AppIdentityUserId",
                table: "Recipes",
                column: "AppIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_AppIdentityUserId",
                table: "Recipes",
                column: "AppIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
