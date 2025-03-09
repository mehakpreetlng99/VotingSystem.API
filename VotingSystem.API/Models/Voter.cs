using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VotingSystem.API.Models
{
    public class Voter
    {
        [Key]
        public Guid VoterId { get; set; }

        [Required, StringLength(100)]
        public string VoterName { get; set; } = string.Empty;

        [Required]
        public int StateId { get; set; }

        public int? CandidateId { get; set; }

        //Navigation Properties
        public State? State { get; set; }
        public Candidate? Candidate { get; set; }
    }
}
