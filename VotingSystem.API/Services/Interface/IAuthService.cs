using System.Threading.Tasks;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services
{
    public interface IAuthService
    {
        Task<string?> AdminLoginAsync(string username, string password);
        Task<Voter?> VoterLoginAsync(string voterCardNumber);

        Task<bool> RegisterAdminAsync(string username, string password);
    }
}
