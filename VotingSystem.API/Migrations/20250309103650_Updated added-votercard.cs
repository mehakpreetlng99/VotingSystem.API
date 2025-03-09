using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class Updatedaddedvotercard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoterCardNumber",
                table: "Voters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoterCardNumber",
                table: "Voters");
        }
    }
}
