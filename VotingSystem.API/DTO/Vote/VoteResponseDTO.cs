namespace VotingSystem.API.DTO.Vote
{
    public class VoteResponseDTO
    {
        public int VoteId { get; set; }
        public string VoterCardNumber { get; set; } = string.Empty;
        public int? CandidateId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public bool Abstained { get; set; }
        public DateTime VoteDate { get; set; }
    }
}
