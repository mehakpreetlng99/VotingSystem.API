using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.DTO.Candidate
{
    public class CandidateRequestDTO
    {
        [Required(ErrorMessage = "Candidate name is required.")]
        [StringLength(100, ErrorMessage = "Candidate name cannot exceed 100 characters.")]
        public string CandidateName { get; set; } = string.Empty;

        [Required(ErrorMessage = "PartyId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "PartyId must be a valid positive integer.")]
        public int PartyId { get; set; }

        [Required(ErrorMessage = "StateId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "StateId must be a valid positive integer.")]
        public int StateId { get; set; }

        //public bool IsActive { get; set; } = true;
    }

}
