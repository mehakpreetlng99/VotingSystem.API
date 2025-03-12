using System.ComponentModel;

namespace VotingSystem.API.DTO.Vote
{
    public class VoteRequestDTO
    {
        public string VoterCardNumber { get; set; } = string.Empty;
        public int? CandidateId { get; set; }

        [DefaultValue(false)]
        public bool IsAbstained { get; set; } 
    }
}
