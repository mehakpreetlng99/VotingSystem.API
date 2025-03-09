using System.Threading.Tasks;
using VotingSystem.API.DTOs;

namespace VotingSystem.API.Services
{
    public interface IAuthService
    {
        Task<string?> AdminLoginAsync(AdminLoginDto loginDto);
        Task<string?> VoterLoginAsync(VoterLoginDto loginDto);
    }
}

