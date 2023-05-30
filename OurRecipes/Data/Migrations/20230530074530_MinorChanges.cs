using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurRecipes.Data.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Units_UnitId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Nutrients_Units_UnitId",
                table: "Nutrients");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Nutrients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Components",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Units_UnitId",
                table: "Components",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrients_Units_UnitId",
                table: "Nutrients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
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

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Nutrients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Components",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
