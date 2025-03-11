using VotingSystem.API.DTO.Candidate;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services
{
    public interface ICandidateService
    {
        Task<List<CandidateResponseDTO>> GetAllCandidatesAsync();
        Task<CandidateResponseDTO?> GetCandidateByIdAsync(int id);
        Task<CandidateResponseDTO> CreateCandidateAsync(CandidateRequestDTO candidateDto);
        Task<CandidateResponseDTO?> UpdateCandidateAsync(int id, CandidateRequestDTO requestDto);
        Task<bool> DeleteCandidateAsync(int id);
    }
}
