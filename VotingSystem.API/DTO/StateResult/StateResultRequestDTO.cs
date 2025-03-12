using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.DTO.StateResult
{
    public class StateResultRequestDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "StateId must be a valid positive integer.")]
        public int StateId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "WinningCandidateId must be a valid positive integer.")]
        public int WinningCandidateId { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "TotalVotes must be a non-negative integer.")]
        public int TotalVotes { get; set; }
    }
}
