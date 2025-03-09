using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.Models
{
    public class VoteController
    {

        [Key]
        public int VoteId { get; set; }

        [ForeignKey("Voter")]
        public Guid VoterId { get; set; }
        

        [ForeignKey("Candidate")]
        public int? CandidateId { get; set; } // Nullable for abstain option

        [Required]
        public int StateId { get; set; }
        public DateTime VoteTime { get; set; }

        //Navigation properties
        public Candidate? Candidate { get; set; }
        public Voter? Voter { get; set; }
        public State? State { get; set; }
    }
}
