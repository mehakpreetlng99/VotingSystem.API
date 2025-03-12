namespace VotingSystem.API.DTO.NationalResult
{
    public class NationalResultResponseDTO
    {
        public string NationalWinnerPartyName { get; set; } = string.Empty;
        public int StatesWon { get; set; }
        public int TotalStates { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
