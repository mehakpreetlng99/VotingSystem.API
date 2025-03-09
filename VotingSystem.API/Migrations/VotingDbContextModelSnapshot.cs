﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VotingSystem.API.Data;

#nullable disable

namespace VotingSystem.API.Migrations
{
    [DbContext(typeof(VotingDbContext))]
    partial class VotingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VotingSystem.API.Models.Candidate", b =>
                {
                    b.Property<int>("CandidateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CandidateId"));

                    b.Property<string>("CandidateName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("PartyId")
                        .HasColumnType("int");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.HasKey("CandidateId");

                    b.HasIndex("PartyId");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("VotingSystem.API.Models.NationalResult", b =>
                {
                    b.Property<int>("NationalResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NationalResultId"));

                    b.Property<int>("PartyId")
                        .HasColumnType("int");

                    b.Property<int>("TotalVotes")
                        .HasColumnType("int");

                    b.HasKey("NationalResultId");

                    b.HasIndex("PartyId");

                    b.ToTable("NationalResults");
                });

            modelBuilder.Entity("VotingSystem.API.Models.Party", b =>
                {
                    b.Property<int>("PartyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PartyId"));

                    b.Property<string>("PartyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PartySymbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PartyId");

                    b.HasIndex("PartySymbol")
                        .IsUnique();

                    b.ToTable("Parties");
                });

            modelBuilder.Entity("VotingSystem.API.Models.State", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StateId"));

                    b.Property<string>("StateName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("StateId");

                    b.ToTable("States");
                });

            modelBuilder.Entity("VotingSystem.API.Models.StateResult", b =>
                {
                    b.Property<int>("StateResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StateResultId"));

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.Property<int>("TotalVotes")
                        .HasColumnType("int");

                    b.Property<int>("WinningCandidateId")
                        .HasColumnType("int");

                    b.HasKey("StateResultId");

                    b.HasIndex("StateId")
                        .IsUnique();

                    b.HasIndex("WinningCandidateId");

                    b.ToTable("StateResults");
                });

            modelBuilder.Entity("VotingSystem.API.Models.Vote", b =>
                {
                    b.Property<int>("VoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VoteId"));

                    b.Property<int?>("CandidateId")
                        .HasColumnType("int");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.Property<Guid>("VoterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("VoteId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("StateId");

                    b.HasIndex("VoterId")
                        .IsUnique();

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("VotingSystem.API.Models.Voter", b =>
                {
                    b.Property<Guid>("VoterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CandidateId")
                        .HasColumnType("int");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.Property<string>("VoterName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("VoterId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("StateId");

                    b.HasIndex("VoterId")
                        .IsUnique();

                    b.ToTable("Voters");
                });

            modelBuilder.Entity("VotingSystem.API.Models.Candidate", b =>
                {
                    b.HasOne("VotingSystem.API.Models.Party", "Party")
                        .WithMany("Candidates")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VotingSystem.API.Models.State", "State")
                        .WithMany("Candidates")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Party");

                    b.Navigation("State");
                });

            modelBuilder.Entity("VotingSystem.API.Models.NationalResult", b =>
                {
                    b.HasOne("VotingSystem.API.Models.Party", "Party")
                        .WithMany()
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Party");
                });

            modelBuilder.Entity("VotingSystem.API.Models.StateResult", b =>
                {
                    b.HasOne("VotingSystem.API.Models.State", "State")
                        .WithOne()
                        .HasForeignKey("VotingSystem.API.Models.StateResult", "StateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VotingSystem.API.Models.Candidate", "WinningCandidate")
                        .WithMany()
                        .HasForeignKey("WinningCandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");

                    b.Navigation("WinningCandidate");
                });

            modelBuilder.Entity("VotingSystem.API.Models.Vote", b =>
                {
                    b.HasOne("VotingSystem.API.Models.Candidate", "Candidate")
                        .WithMany("Votes")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("VotingSystem.API.Models.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VotingSystem.API.Models.Voter", "Voter")
                        .WithOne()
                        .HasForeignKey("VotingSystem.API.Models.Vote", "VoterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("State");

                    b.Navigation("Voter");
                });

            modelBuilder.Entity("VotingSystem.API.Models.Voter", b =>
                {
                    b.HasOne("VotingSystem.API.Models.Candidate", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId");

                    b.HasOne("VotingSystem.API.Models.State", "State")
                        .WithMany("Voters")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("State");
                });

            modelBuilder.Entity("VotingSystem.API.Models.Candidate", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("VotingSystem.API.Models.Party", b =>
                {
                    b.Navigation("Candidates");
                });

            modelBuilder.Entity("VotingSystem.API.Models.State", b =>
                {
                    b.Navigation("Candidates");

                    b.Navigation("Voters");
                });
#pragma warning restore 612, 618
        }
    }
}
