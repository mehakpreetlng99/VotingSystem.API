//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using VotingSystem.API.Data;
//using VotingSystem.API.DTO
//using VotingSystem.API.Models;

//namespace VotingSystem.API.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly VotingDbContext _context;
//        private readonly IConfiguration _configuration;

//        public AuthService(VotingDbContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }

//        public async Task<string?> AuthenticateAdminAsync(AdminLoginDto adminLoginDto)
//        {
//            var admin = await _context.Admin
//                .FirstOrDefaultAsync(a => a.Username == adminLoginDto.Username && a.Password == adminLoginDto.Password);

//            if (admin == null)
//                return null;

//            return GenerateJwtToken(admin.Username, "Admin");
//        }

//        public async Task<string?> AuthenticateVoterAsync(VoterLoginDto voterLoginDto)
//        {
//            var voter = await _context.Voters
//                .FirstOrDefaultAsync(v => v.VoterCardNumber == voterLoginDto.VoterCardNumber);

//            if (voter == null)
//                return null;

//            return GenerateJwtToken(voter.VoterCardNumber, "Voter");
//        }

//        private string GenerateJwtToken(string identifier, string role)
//        {
//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//                new Claim(ClaimTypes.Name, identifier),
//                new Claim(ClaimTypes.Role, role)
//            };

//            var token = new JwtSecurityToken(
//                _configuration["Jwt:Issuer"],
//                _configuration["Jwt:Audience"],
//                claims,
//                expires: DateTime.UtcNow.AddHours(2),
//                signingCredentials: credentials);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
