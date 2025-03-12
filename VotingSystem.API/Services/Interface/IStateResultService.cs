using System.Threading.Tasks;
using VotingSystem.API.DTO.StateResult;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services.Interface
{
    public interface IStateResultService
    {

        Task<StateResultResponseDTO> DeclareWinnerAsync(int stateId);
        // Method to get state result by state ID
        Task<StateResultResponseDTO> GetStateResultByStateIdAsync(int stateId);
    }
}
