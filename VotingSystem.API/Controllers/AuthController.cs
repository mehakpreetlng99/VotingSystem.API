
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VotingSystem.API.DTO;
using VotingSystem.API.Services;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IVoterService _voterService;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration config, IVoterService voterService, IAuthService authService)
    {
        _config = config;
        _voterService = voterService;
        _authService = authService;
    }

    // 🔹 Admin Login
    [HttpPost("admin/login")]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto adminLoginDto)
    {
        var token = await _authService.AdminLoginAsync(adminLoginDto.Username, adminLoginDto.Password);

        if (token == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(new { Token = token });
    }

    // 🔹 Voter Login
    [HttpPost("voter/login")]
    public async Task<IActionResult> VoterLogin([FromBody] VoterLoginDto voterLoginDto)
    {
        var voter = await _voterService.GetVoterByCardNumberAsync(voterLoginDto.VoterCardNumber);
        if (voter == null)
        {
            return Unauthorized("Invalid Voter Card Number");
        }

        var token = GenerateJwtToken(voter.VoterCardNumber, "Voter");
        return Ok(new { Token = token });
    }

    // 🔹 Admin Registration
    [HttpPost("admin/register")]
    public async Task<IActionResult> RegisterAdmin([FromBody] AdminLoginDto adminLoginDto)
    {
        var success = await _authService.RegisterAdminAsync(adminLoginDto.Username, adminLoginDto.Password);
        if (!success)
        {
            return BadRequest("Admin already exists");
        }

        return Ok("Admin registered successfully");
    }

    // 🔹 JWT Token Generator
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

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using VotingSystem.API.DTO;
//using VotingSystem.API.Services;

//[Route("api/auth")]
//[ApiController]
//public class AuthController : ControllerBase
//{
//    private readonly IConfiguration _config;
//    private readonly IVoterService _voterService; 
//    private readonly IAuthService _authService;

//    public AuthController(IConfiguration config, IVoterService voterService ,IAuthService authService)
//    {
//        _config = config;
//        _voterService = voterService;
//        _authService = authService;
//    }

//    [HttpPost("admin/login")]
//    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto adminLoginDto)
//    {
//        var token = await _authService.AdminLoginAsync(adminLoginDto.Username, adminLoginDto.Password);
//        if (token == null)
//            return Unauthorized("Invalid credentials");

//        return Ok(new { Token = token });
//    }

//    [HttpPost("voter/login")]
//    public async Task<IActionResult> VoterLogin([FromBody] VoterLoginDto voterLoginDto)
//    {
//        var voter = await _voterService.GetVoterByCardNumberAsync(voterLoginDto.VoterCardNumber);
//        if (voter == null)
//        {
//            return Unauthorized("Invalid Voter Card Number");
//        }

//        var token = GenerateJwtToken(voter.VoterCardNumber, "Voter");
//        return Ok(new { Token = token });
//    }
//    [HttpPost("admin/register")]
//    public async Task<IActionResult> RegisterAdmin([FromBody] AdminLoginDto adminLoginDto)
//    {
//        var success = await _authService.RegisterAdminAsync(adminLoginDto.Username, adminLoginDto.Password);
//        if (!success)
//            return BadRequest("Admin already exists");

//        return Ok("Admin registered successfully");
//    }
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
