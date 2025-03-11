namespace VotingSystem.API.DTO.Voter
{
    public class VoterResponseDTO
    {
        public Guid VoterId { get; set; }
        public string VoterName { get; set; }= string.Empty;
        public string VoterCardNumber { get; set; }= string.Empty;
    }
}
