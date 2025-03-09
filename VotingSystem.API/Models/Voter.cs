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

        [Required]
        public string VoterCardNumber { get; set; } = string.Empty;

        [Required]
        [Range(18,120,ErrorMessage="Age must be between 18 and 120")]
        public int Age { get; set; }

        //Navigation Properties
        [ForeignKey("StateId")]
        public State? State { get; set; }
        public Candidate? Candidate { get; set; }
    }
}
