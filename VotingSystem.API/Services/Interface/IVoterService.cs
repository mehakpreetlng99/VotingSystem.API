using VotingSystem.API.Models;
namespace VotingSystem.API.Services
{
    public interface IVoterService
    {
        Task<IEnumerable<Voter>> AllVotersAsync();

        Task<Voter?> GetVoterByIdAsync(Guid voterId);
        Task<bool> RegisterVoterAsync(Voter voter);
        Task<bool> CastVoteAsync(Guid voterId, int candidateId);
        Task<Voter?> GetVoterByCardNumberAsync(string voterCardNumber);
    }
}
