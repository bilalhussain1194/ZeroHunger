using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kill_hunger.Migrations
{
    /// <inheritdoc />
    public partial class edit_filepath_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "FileDetails");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "FileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "FileDetails");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "FileDetails",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
