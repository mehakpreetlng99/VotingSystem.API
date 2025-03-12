
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


