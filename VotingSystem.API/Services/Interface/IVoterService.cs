//using VotingSystem.API.Models;
//namespace VotingSystem.API.Services
//{
//    public interface IVoterService
//    {
//        Task<IEnumerable<Voter>> AllVotersAsync();

//        Task<Voter?> GetVoterByIdAsync(Guid voterId);
//        Task<bool> RegisterVoterAsync(Voter voter);
//        Task<bool> CastVoteAsync(Guid voterId, int candidateId);
//        Task<Voter?> GetVoterByCardNumberAsync(string voterCardNumber);
//    }
//}

using VotingSystem.API.DTO.Voter;

public interface IVoterService
{
    Task<VoterResponseDTO> RegisterVoterAsync(VoterRequestDTO voterDto);
    Task<IEnumerable<VoterResponseDTO>> GetAllVotersAsync(); 
    Task<VoterResponseDTO> GetVoterByCardNumberAsync(string voterCardNumber); 
    Task<VoterResponseDTO> UpdateVoterAsync(string voterCardNumber, VoterRequestDTO voterDto); 
    Task<IEnumerable<VoterResponseDTO>> GetVotersByStateIdAsync(int stateId); 
}
