using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VotingSystem.API.Models;
using VotingSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Services;

public class AuthService : IAuthService
{
    private readonly VotingDbContext _context;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(VotingDbContext context, IConfiguration config, ILogger<AuthService> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    // 🔹 Admin Login with Secure Password Hashing
    public async Task<string?> AdminLoginAsync(string username, string password)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);

        if (admin == null)
        {
            _logger.LogWarning("Admin login failed: Admin not found for username '{Username}'", username);
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, admin.Password))
        {
            _logger.LogWarning("Admin login failed: Incorrect password for username '{Username}'", username);
            return null;
        }

        _logger.LogInformation("Admin '{Username}' successfully logged in.", username);
        return GenerateJwtToken(username, "Admin");
    }

    // 🔹 Voter Login
    public async Task<Voter?> VoterLoginAsync(string voterCardNumber)
    {
        var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);
        if (voter == null)
        {
            _logger.LogWarning("Voter login failed: No voter found with VoterCardNumber '{VoterCardNumber}'", voterCardNumber);
        }
        else
        {
            _logger.LogInformation("Voter '{VoterCardNumber}' successfully logged in.", voterCardNumber);
        }
        return voter;
    }

    // 🔹 Secure Admin Registration
    public async Task<bool> RegisterAdminAsync(string username, string password)
    {
        if (await _context.Admins.AnyAsync(a => a.Username == username))
        {
            _logger.LogWarning("Admin registration failed: Username '{Username}' already exists.", username);
            return false;
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var admin = new Admin
        {
            Username = username,
            Password = hashedPassword
        };

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Admin '{Username}' successfully registered.", username);
        return true;
    }

    /// <summary>
    /// 🔹 Generate JWT Token
    /// </summary>
    private string GenerateJwtToken(string identifier, string role)
    {
        try
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

            _logger.LogInformation("JWT token generated for '{Identifier}' with role '{Role}'", identifier, role);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating JWT token for '{Identifier}'", identifier);
            throw;
        }
    }
}


//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using VotingSystem.API.Models;
//using VotingSystem.API.Data;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;
//using VotingSystem.API.Services;

//public class AuthService : IAuthService
//{
//    private readonly VotingDbContext _context;
//    private readonly IConfiguration _config;

//    public AuthService(VotingDbContext context, IConfiguration config)
//    {
//        _context = context;
//        _config = config;
//    }

//    // 🔹 Admin Login with Secure Password Hashing
//    public async Task<string?> AdminLoginAsync(string username, string password)
//    {
//        var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);

//        if (admin == null)
//        {
//            Console.WriteLine("❌ Admin not found");
//            return null;
//        }

//        // Check hashed password
//        if (!BCrypt.Net.BCrypt.Verify(password, admin.Password))
//        {
//            Console.WriteLine("❌ Password mismatch");
//            return null;
//        }

//        // Generate JWT Token
//        return GenerateJwtToken(username, "Admin");
//    }

//    // 🔹 Voter Login
//    public async Task<Voter?> VoterLoginAsync(string voterCardNumber)
//    {
//        return await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);
//    }

//    // 🔹 Secure Admin Registration
//    public async Task<bool> RegisterAdminAsync(string username, string password)
//    {
//        if (await _context.Admins.AnyAsync(a => a.Username == username))
//            return false; // Admin already exists

//        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password); // Hash password
//        var admin = new Admin
//        {
//            Username = username,
//            Password = hashedPassword
//        };

//        _context.Admins.Add(admin);
//        await _context.SaveChangesAsync();
//        return true;
//    }

//    /// <summary>
//    /// 🔹 Generate JWT Token
//    /// </summary>
//    private string GenerateJwtToken(string identifier, string role)
//    {
//        var secretKey = _config["JWT_SECRET"];
//        var issuer = _config["JWT_ISSUER"];
//        var audience = _config["JWT_AUDIENCE"];

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var claims = new[]
//        {
//            new Claim(JwtRegisteredClaimNames.Sub, identifier),
//            new Claim(ClaimTypes.Role, role),
//            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//        };

//        var token = new JwtSecurityToken(
//            issuer: issuer,
//            audience: audience,
//            claims: claims,
//            expires: DateTime.UtcNow.AddHours(1),
//            signingCredentials: creds
//        );

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//}


