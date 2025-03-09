﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class Updatedaddeddatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VoteTime",
                table: "Votes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteTime",
                table: "Votes");
        }
    }
}
