using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kill_hunger.Migrations
{
    /// <inheritdoc />
    public partial class Addcolumn_IsClose_CloseReason_In_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloseReason",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClose",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseReason",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsClose",
                table: "Requests");
        }
    }
}
