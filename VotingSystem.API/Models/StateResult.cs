using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.Models
{
    public class StateResult
    {
        [Key]
        public int StateResultId { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }

        [Required]
        public int WinningCandidateId { get; set; }

        [Required]
        public int TotalVotes { get; set; }

        //Navigation Property
        public State? State { get; set; }
        public Candidate? WinningCandidate { get; set; }
    }
}
