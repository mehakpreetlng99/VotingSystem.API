
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VotingSystem.API.DTO.Vote;
using VotingSystem.API.Services;

[Route("api/[controller]")]
[ApiController]
public class VoteController : ControllerBase
{
    private readonly IVoteService _voteService;
    private readonly ILogger<VoteController> _logger;

    public VoteController(IVoteService voteService, ILogger<VoteController> logger)
    {
        _voteService = voteService;
        _logger = logger;
    }

    [HttpPost("cast")]
    public async Task<IActionResult> CastVote([FromBody] VoteRequestDTO voteDto)
    {
        if (voteDto == null)
        {
            _logger.LogWarning("Invalid vote request received.");
            return BadRequest("Invalid vote request.");
        }

        try
        {
            _logger.LogInformation("Attempting to cast a vote for VoterCardNumber: {VoterCardNumber}", voteDto.VoterCardNumber);
            bool isVoteCast = await _voteService.CastVoteAsync(voteDto);

            if (isVoteCast)
            {
                _logger.LogInformation("Vote successfully cast for VoterCardNumber: {VoterCardNumber}", voteDto.VoterCardNumber);
                return Ok(new
                {
                    Message = voteDto.IsAbstained
                        ? "You have successfully abstained from voting."
                        : "Vote cast successfully."
                });
            }

            _logger.LogWarning("Vote casting failed for VoterCardNumber: {VoterCardNumber}", voteDto.VoterCardNumber);
            return BadRequest("Vote could not be cast. Please try again.");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Voter not found while casting vote: {VoterCardNumber}", voteDto.VoterCardNumber);
            return NotFound(new { Error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized vote attempt by VoterCardNumber: {VoterCardNumber}", voteDto.VoterCardNumber);
            return Unauthorized(new { Error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Invalid operation while casting vote: {Error}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while casting the vote.");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }

    [HttpGet("total-votes")]
    public async Task<IActionResult> GetTotalVotes()
    {
        try
        {
            _logger.LogInformation("Fetching total vote count.");
            var totalVotes = await _voteService.GetTotalVotesAsync();
            return Ok(new { TotalVotes = totalVotes });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching total votes.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("votes-by-state/{stateId}")]
    public async Task<IActionResult> GetVotesByState(int stateId)
    {
        try
        {
            _logger.LogInformation("Fetching votes for StateId: {StateId}", stateId);
            var stateVotes = await _voteService.GetVotesByStateAsync(stateId);
            return Ok(new { StateId = stateId, Votes = stateVotes });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching votes by state.");
            return BadRequest(ex.Message);
        }
    }
}


