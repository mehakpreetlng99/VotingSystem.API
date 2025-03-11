namespace VotingSystem.API.DTO.Voter
{
    public class VoterRequestDTO
    {
        public string VoterName { get; set; } = string.Empty;
        public int Age { get; set; }
        public int StateId { get; set; }
    }

}
