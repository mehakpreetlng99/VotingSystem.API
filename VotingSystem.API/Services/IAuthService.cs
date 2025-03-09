using System.Threading.Tasks;
using VotingSystem.API.DTO;

namespace VotingSystem.API.Services
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAdminAsync(AdminLoginDto adminLoginDto);
        Task<string?> AuthenticateVoterAsync(VoterLoginDto voterLoginDto);
    }
}
