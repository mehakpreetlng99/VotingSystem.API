namespace VotingSystem.API.DTO
{
    public class CandidateDTO
    {
        public string Name { get; set; } = string.Empty;
        public int PartyId { get; set; }
        public int StateId { get; set; }
    }
}
