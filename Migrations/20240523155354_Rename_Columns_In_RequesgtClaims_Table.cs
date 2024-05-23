using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kill_hunger.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Columns_In_RequesgtClaims_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discrition",
                table: "RequestClaims",
                newName: "Discription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "RequestClaims",
                newName: "Discrition");
        }
    }
}
