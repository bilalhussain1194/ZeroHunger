using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kill_hunger.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discription",
                table: "Requests",
                newName: "Discription");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "Requests",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "Requests",
                newName: "City");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "Requests",
                newName: "discription");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Requests",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Requests",
                newName: "city");
        }
    }
}
