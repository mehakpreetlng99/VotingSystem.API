namespace VotingSystem.API.DTO
{
    public class ElectionResultDTO
    {
        public string WinningParty { get; set; } = string.Empty;
        public int TotalVotes { get; set; }
    }
}
