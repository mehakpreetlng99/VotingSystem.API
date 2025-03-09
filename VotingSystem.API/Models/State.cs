using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Required, StringLength(100)]
        public string StateName { get; set; } = string.Empty;

        // Navigation property
        public List<Voter> Voters { get; set; } = new List<Voter>();
        public List<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
}
