namespace VotingSystem.API.DTO.Vote
{
    public class VoteRequestDTO
    {
        public string VoterCardNumber { get; set; } = string.Empty;
        public int? CandidateId { get; set; }

        public bool IsAbstained { get; set; }
    }
}
