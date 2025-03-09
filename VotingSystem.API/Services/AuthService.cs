using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VotingSystem.API.DTO;
using VotingSystem.API.Models;
using VotingSystem.API.Data;
using VotingSystem.API.Services;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly VotingDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(VotingDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string?> AdminLoginAsync(string username, string password)
    {
        var admin = _context.Admins.FirstOrDefault(a => a.Username == username && a.Password == password);
        if (admin == null) return null;

        return GenerateJwtToken(username, "Admin");
    }

   
    public async Task<Voter?> VoterLoginAsync(string voterCardNumber)
    {
        return _context.Voters.FirstOrDefault(v => v.VoterCardNumber == voterCardNumber);
    }

    public async Task<bool> RegisterAdminAsync(string username, string password)
    {
        if (await _context.Admins.AnyAsync(a => a.Username == username))
            return false;

        var admin = new Admin
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();
        return true;
    }
    /// <summary>
    /// Generate JWT Token
    /// </summary>
    private string GenerateJwtToken(string identifier, string role)
    {
        var secretKey = _config["JWT_SECRET"];
        var issuer = _config["JWT_ISSUER"];
        var audience = _config["JWT_AUDIENCE"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identifier),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
