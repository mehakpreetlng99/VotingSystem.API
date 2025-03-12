using VotingSystem.API.DTO.NationalResult;

namespace VotingSystem.API.Services.Interface
{
    public interface INationalResultService
    {
        Task<NationalResultResponseDTO> GetNationalResultAsync();
    }
}
