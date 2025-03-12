
using Microsoft.EntityFrameworkCore;
using VotingSystem.API.Models;
namespace VotingSystem.API.Data
{
    public class VotingDbContext :DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> options) : base(options)
        {
        }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<StateResult> StateResults { get; set; }
        public DbSet<NationalResult> NationalResults { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //unique constarints
            modelBuilder.Entity<Voter>().HasIndex(v => v.VoterId).IsUnique();
            modelBuilder.Entity<Party>().HasIndex(p => p.PartySymbol).IsUnique();
            modelBuilder.Entity<Admin>().HasIndex(a => a.Username).IsUnique();

            // candidate-state
            modelBuilder.Entity<Candidate>().HasOne(c => c.State).WithMany(s => s.Candidates).HasForeignKey(c => c.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            //candidate-party
            modelBuilder.Entity<Candidate>().HasOne(c => c.Party).WithMany(p => p.Candidates).HasForeignKey(c => c.PartyId)
                .OnDelete(DeleteBehavior.Restrict);

            //vote-voter
            modelBuilder.Entity<Vote>().HasOne(v => v.Voter).WithOne().HasForeignKey<Vote>(v => v.VoterId)
                .OnDelete(DeleteBehavior.Restrict);

            //vote-candidate
            modelBuilder.Entity<Vote>().HasOne(v => v.Candidate).WithMany(c => c.Votes).HasForeignKey(v => v.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            //Stateresult-state
            modelBuilder.Entity<StateResult>().HasOne(sr => sr.State).WithOne().HasForeignKey<StateResult>(sr => sr.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            //NationalREsult-Party
            modelBuilder.Entity<NationalResult>().HasOne(nr => nr.Party).WithMany().HasForeignKey(nr => nr.PartyId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
