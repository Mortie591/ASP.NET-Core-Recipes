using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurRecipes.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnitsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Components");

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "Nutrients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Nutrients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Components",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_UnitId",
                table: "Nutrients",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_UnitId",
                table: "Components",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Units_UnitId",
                table: "Components",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrients_Units_UnitId",
                table: "Nutrients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Units_UnitId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Nutrients_Units_UnitId",
                table: "Nutrients");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Nutrients_UnitId",
                table: "Nutrients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Components_UnitId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Nutrients");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Components");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "Nutrients",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Components",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
