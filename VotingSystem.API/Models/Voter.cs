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

        [Required, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public bool HasVoted { get; set; } = false;
    }
}
