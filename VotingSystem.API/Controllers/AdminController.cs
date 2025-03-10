using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.API.DTO;
using VotingSystem.API.Models;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // Candidate Management
    [HttpPost("add-candidate")]
    public async Task<IActionResult> AddCandidate([FromBody] CandidateDTO candidateDto)
    {
        var result = await _adminService.AddCandidateAsync(candidateDto);
        if (result) return Ok("Candidate added successfully.");
        return BadRequest("Failed to add candidate.");
    }

    [HttpGet("candidates")]
    public async Task<ActionResult<List<Candidate>>> GetAllCandidates()
    {
        var candidates = await _adminService.GetAllCandidatesAsync();
        return Ok(candidates);
    }

    // Voter Management
    [HttpGet("voters")]
    public async Task<ActionResult<List<Voter>>> GetAllVoters()
    {
        var voters = await _adminService.GetAllVotersAsync();
        return Ok(voters);
    }

    // Election Result Management
    [HttpPost("update-state-result")]
    public async Task<IActionResult> UpdateStateResult([FromBody] StateResultDTO stateResultDto)
    {
        var result = await _adminService.UpdateStateResultAsync(stateResultDto);
        if (result) return Ok("State result updated successfully.");
        return BadRequest("Failed to update state result.");
    }

    [HttpPost("update-national-result")]
    public async Task<IActionResult> UpdateNationalResult([FromBody] NationalResultDTO nationalResultDto)
    {
        var result = await _adminService.UpdateNationalResultAsync(nationalResultDto);
        if (result) return Ok("National result updated successfully.");
        return BadRequest("Failed to update national result.");
    }

    [HttpGet("state-results")]
    public async Task<ActionResult<List<StateResult>>> GetStateResults()
    {
        var results = await _adminService.GetStateResultsAsync();
        return Ok(results);
    }

    [HttpGet("national-result")]
    public async Task<ActionResult<NationalResult>> GetNationalResult()
    {
        var result = await _adminService.GetNationalResultAsync();
        if (result != null) return Ok(result);
        return NotFound("No national result found.");
    }
}
