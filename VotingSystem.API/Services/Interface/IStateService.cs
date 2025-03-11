using VotingSystem.API.DTO.State;

namespace VotingSystem.API.Services.Interface
{
    public interface IStateService
    {
        Task<StateResponseDTO> CreateStateAsync(StateRequestDTO stateDto);
        Task<IEnumerable<StateResponseDTO>> GetAllStatesAsync();
        Task<StateResponseDTO?> GetStateByIdAsync(int stateId);
        Task<bool> DeleteStateAsync(int stateId);
        Task<StateResponseDTO?> UpdateStateAsync(int stateId, StateRequestDTO stateDto);
    }
}
