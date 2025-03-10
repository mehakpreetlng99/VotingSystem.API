namespace VotingSystem.API.DTO
{
    public class StateResultDTO
    {
        public int StateId { get; set; }
        public int WinningCandidateId { get; set; }
        public int TotalVotes { get; set; }
    }
}
