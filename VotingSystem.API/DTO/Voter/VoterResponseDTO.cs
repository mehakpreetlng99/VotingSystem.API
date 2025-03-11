using System.Globalization;

namespace VotingSystem.API.DTO.Voter
{
    public class VoterResponseDTO
    {
        public Guid VoterId { get; set; }
        public string VoterName { get; set; }= string.Empty;
        public string VoterCardNumber { get; set; }= string.Empty;
        public int Age { get; set; }

        public int StateId { get; set; }
    }
}
