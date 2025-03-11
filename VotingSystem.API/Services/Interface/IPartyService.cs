using VotingSystem.API.DTO.Party;
namespace VotingSystem.API.Services
{
    public interface IPartyService
    {
        Task<PartyResponseDTO> CreatePartyAsync(PartyRequestDTO partyDto);
        Task<IEnumerable<PartyResponseDTO>> GetAllPartiesAsync();
        Task<PartyResponseDTO?> GetPartyByIdAsync(int partyId);
        Task<bool> DeletePartyAsync(int partyId);
        Task<PartyResponseDTO?> UpdatePartyAsync(int partyId, PartyRequestDTO partyDto);
    }
}
