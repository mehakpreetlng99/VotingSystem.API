
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.API.DTO.Voter;
using VotingSystem.API.Services;

[Route("api/voter")]
[ApiController]
public class VoterController : ControllerBase
{
    private readonly IVoterService _voterService;
    private readonly ILogger<VoterController> _logger;

    public VoterController(IVoterService voterService, ILogger<VoterController> logger)
    {
        _voterService = voterService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterVoter([FromBody] VoterRequestDTO voterDto)
    {
        try
        {
            _logger.LogInformation("Registering a new voter.");
            var result = await _voterService.RegisterVoterAsync(voterDto);
            _logger.LogInformation("Voter registered successfully: {VoterId}", result.VoterId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Voter registration failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during voter registration.");
            return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllVoters()
    {
        _logger.LogInformation("Fetching all voters.");
        var voters = await _voterService.GetAllVotersAsync();
        return Ok(voters);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{voterCardNumber}")]
    public async Task<IActionResult> GetVoterByCardNumber(string voterCardNumber)
    {
        try
        {
            _logger.LogInformation("Fetching voter details for VoterCardNumber: {VoterCardNumber}", voterCardNumber);
            var voter = await _voterService.GetVoterByCardNumberAsync(voterCardNumber);
            return Ok(voter);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Voter not found: {VoterCardNumber}", voterCardNumber);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching voter details.");
            return StatusCode(500, new { message = "An error occurred while processing the request." });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{voterCardNumber}")]
    public async Task<IActionResult> UpdateVoter(string voterCardNumber, [FromBody] VoterRequestDTO voterDto)
    {
        try
        {
            _logger.LogInformation("Updating voter details for VoterCardNumber: {VoterCardNumber}", voterCardNumber);
            var updatedVoter = await _voterService.UpdateVoterAsync(voterCardNumber, voterDto);
            if (updatedVoter == null)
            {
                _logger.LogWarning("Voter not found for update: {VoterCardNumber}", voterCardNumber);
                return NotFound("Voter not found.");
            }

            _logger.LogInformation("Voter details updated successfully for VoterCardNumber: {VoterCardNumber}", voterCardNumber);
            return Ok(updatedVoter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating voter details.");
            return StatusCode(500, new { message = "An error occurred while updating the voter details." });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("state/{stateId}")]
    public async Task<IActionResult> GetVotersByState(int stateId)
    {
        _logger.LogInformation("Fetching voters for StateId: {StateId}", stateId);
        var voters = await _voterService.GetVotersByStateIdAsync(stateId);
        return Ok(voters);
    }
}

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using VotingSystem.API.DTO.Voter;
//using VotingSystem.API.Services;

//[Route("api/voter")]
//[ApiController]
//public class VoterController : ControllerBase
//{
//    private readonly IVoterService _voterService;

//    public VoterController(IVoterService voterService)
//    {
//        _voterService = voterService;
//    }

//    // ✅ Register Voter (Public)

//    [HttpPost("register")]
//    public async Task<IActionResult> RegisterVoter([FromBody] VoterRequestDTO voterDto)
//    {
//        try
//        {
//            var result = await _voterService.RegisterVoterAsync(voterDto);
//            return Ok(result);
//        }
//        catch (ArgumentException ex)
//        {
//            return BadRequest(new { message = ex.Message }); // ✅ Returns only the short error message
//        }
//        catch (Exception)
//        {
//            return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
//        }
//    }


//    // ✅ Get All Voters (Admin Only)
//    [Authorize(Roles = "Admin")]
//    [HttpGet("all")]
//    public async Task<IActionResult> GetAllVoters()
//    {
//        var voters = await _voterService.GetAllVotersAsync();
//        return Ok(voters);
//    }

//    // ✅ Get Voter by ID (Admin Only)
//    [Authorize(Roles = "Admin")]
//    [HttpGet("{voterCardNumber}")]
//    public async Task<IActionResult> GetVoterByCardNumber(string voterCardNumber)
//    {
//        try
//        {
//            var voter = await _voterService.GetVoterByCardNumberAsync(voterCardNumber);
//            return Ok(voter);
//        }
//        catch (KeyNotFoundException ex)
//        {
//            return NotFound(new { message = ex.Message });
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, new { message = "An error occurred while processing the request.", details = ex.Message });
//        }
//    }

//    //[Authorize(Roles = "Admin")]
//    //[HttpGet("{voterId}")]
//    //public async Task<IActionResult> GetVoterById(Guid voterId)
//    //{
//    //    var voter = await _voterService.GetVoterByIdAsync(voterId);
//    //    if (voter == null)
//    //        return NotFound("Voter not found.");

//    //    return Ok(voter);
//    //}

//    // ✅ Update Voter (Admin Only)
//    //[Authorize(Roles = "Admin")]
//    //[HttpPut("{voterId}")]
//    //public async Task<IActionResult> UpdateVoter(string voterCardNumber, [FromBody] VoterRequestDTO voterDto)
//    //{
//    //    var updatedVoter = await _voterService.UpdateVoterAsync(voterCardNumber, voterDto);
//    //    if (updatedVoter == null)
//    //        return NotFound("Voter not found.");

//    //    return Ok(updatedVoter);
//    //}
//    [Authorize(Roles = "Admin")]
//    [HttpPut("{voterCardNumber}")]
//    public async Task<IActionResult> UpdateVoter(string voterCardNumber, [FromBody] VoterRequestDTO voterDto)
//    {
//        var updatedVoter = await _voterService.UpdateVoterAsync(voterCardNumber, voterDto);
//        if (updatedVoter == null)
//            return NotFound("Voter not found.");

//        return Ok(updatedVoter);
//    }


//    // ✅ Get Voters by State ID (Admin Only)
//    [Authorize(Roles = "Admin")]
//    [HttpGet("state/{stateId}")]
//    public async Task<IActionResult> GetVotersByState(int stateId)
//    {
//        var voters = await _voterService.GetVotersByStateIdAsync(stateId);
//        return Ok(voters);
//    }
//}
