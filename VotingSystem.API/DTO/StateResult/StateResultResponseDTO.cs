namespace VotingSystem.API.DTO.StateResult
{
    public class StateResultResponseDTO
    {
        public int StateResultId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; } = string.Empty;
        public int WinningCandidateId { get; set; }
        public string WinningCandidateName { get; set; } = string.Empty;
        public int TotalVotes { get; set; }

       
    }
}
