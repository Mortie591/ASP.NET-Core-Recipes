using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurRecipes.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnitAbbr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Units");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
