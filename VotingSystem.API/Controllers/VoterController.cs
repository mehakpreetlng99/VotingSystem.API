using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.API.DTO.Voter;
using VotingSystem.API.Services;

[Route("api/voter")]
[ApiController]
public class VoterController : ControllerBase
{
    private readonly IVoterService _voterService;

    public VoterController(IVoterService voterService)
    {
        _voterService = voterService;
    }

    // ✅ Register Voter (Public)

    [HttpPost("register")]
    public async Task<IActionResult> RegisterVoter([FromBody] VoterRequestDTO voterDto)
    {
        try
        {
            var result = await _voterService.RegisterVoterAsync(voterDto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message }); // ✅ Returns only the short error message
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
        }
    }


    // ✅ Get All Voters (Admin Only)
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllVoters()
    {
        var voters = await _voterService.GetAllVotersAsync();
        return Ok(voters);
    }

    // ✅ Get Voter by ID (Admin Only)
    [Authorize(Roles = "Admin")]
    [HttpGet("{voterCardNumber}")]
    public async Task<IActionResult> GetVoterByCardNumber(string voterCardNumber)
    {
        try
        {
            var voter = await _voterService.GetVoterByCardNumberAsync(voterCardNumber);
            return Ok(voter);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing the request.", details = ex.Message });
        }
    }

    //[Authorize(Roles = "Admin")]
    //[HttpGet("{voterId}")]
    //public async Task<IActionResult> GetVoterById(Guid voterId)
    //{
    //    var voter = await _voterService.GetVoterByIdAsync(voterId);
    //    if (voter == null)
    //        return NotFound("Voter not found.");

    //    return Ok(voter);
    //}

    // ✅ Update Voter (Admin Only)
    //[Authorize(Roles = "Admin")]
    //[HttpPut("{voterId}")]
    //public async Task<IActionResult> UpdateVoter(string voterCardNumber, [FromBody] VoterRequestDTO voterDto)
    //{
    //    var updatedVoter = await _voterService.UpdateVoterAsync(voterCardNumber, voterDto);
    //    if (updatedVoter == null)
    //        return NotFound("Voter not found.");

    //    return Ok(updatedVoter);
    //}
    [Authorize(Roles = "Admin")]
    [HttpPut("{voterCardNumber}")]
    public async Task<IActionResult> UpdateVoter(string voterCardNumber, [FromBody] VoterRequestDTO voterDto)
    {
        var updatedVoter = await _voterService.UpdateVoterAsync(voterCardNumber, voterDto);
        if (updatedVoter == null)
            return NotFound("Voter not found.");

        return Ok(updatedVoter);
    }


    // ✅ Get Voters by State ID (Admin Only)
    [Authorize(Roles = "Admin")]
    [HttpGet("state/{stateId}")]
    public async Task<IActionResult> GetVotersByState(int stateId)
    {
        var voters = await _voterService.GetVotersByStateIdAsync(stateId);
        return Ok(voters);
    }
}
