

using VotingSystem.API.DTO.Voter;

public interface IVoterService
{
    Task<VoterResponseDTO> RegisterVoterAsync(VoterRequestDTO voterDto);
    Task<IEnumerable<VoterResponseDTO>> GetAllVotersAsync(); 
    Task<VoterResponseDTO> GetVoterByCardNumberAsync(string voterCardNumber); 
    Task<VoterResponseDTO> UpdateVoterAsync(string voterCardNumber, VoterRequestDTO voterDto); 
    Task<IEnumerable<VoterResponseDTO>> GetVotersByStateIdAsync(int stateId); 
}
