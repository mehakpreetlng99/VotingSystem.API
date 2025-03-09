using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.DTO
{
    public class VoterLoginDto
    {
        [Required(ErrorMessage = "Voter Card Number is required")]
        public string VoterCardNumber { get; set; } = string.Empty;
    }
}
