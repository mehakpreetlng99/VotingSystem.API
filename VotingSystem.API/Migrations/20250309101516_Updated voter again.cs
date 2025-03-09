using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class Updatedvoteragain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Voters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Voters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
