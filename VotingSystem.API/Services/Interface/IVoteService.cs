using System.Threading.Tasks;
using VotingSystem.API.DTO.Candidate;
using VotingSystem.API.DTO.Vote;

namespace VotingSystem.API.Services
{
    public interface IVoteService
    {
        Task<bool> CastVoteAsync(VoteRequestDTO voteDto);
        Task<List<CandidateResponseDTO>> GetCandidatesByVoterStateAsync(string voterCardNumber);
        Task<int> GetTotalVotesAsync();
        Task<int> GetVotesByStateAsync(int stateId);
    }
}
