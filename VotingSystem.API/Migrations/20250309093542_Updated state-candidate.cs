using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class Updatedstatecandidate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_States_PartyId",
                table: "Candidates");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_StateId",
                table: "Candidates",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_States_StateId",
                table: "Candidates",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_States_StateId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_StateId",
                table: "Candidates");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_States_PartyId",
                table: "Candidates",
                column: "PartyId",
                principalTable: "States",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
