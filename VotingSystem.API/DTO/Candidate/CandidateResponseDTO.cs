namespace VotingSystem.API.DTO.Candidate
{
    public class CandidateResponseDTO
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public int PartyId { get; set; }
        public string PartyName { get; set; } = string.Empty; 
        public int StateId { get; set; }
        public string StateName { get; set; } = string.Empty; 
        //public bool IsActive { get; set; }
    }

}
