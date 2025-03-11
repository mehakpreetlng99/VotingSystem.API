namespace VotingSystem.API.DTO.Party
{
    public class PartyResponseDTO
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
    }
}
