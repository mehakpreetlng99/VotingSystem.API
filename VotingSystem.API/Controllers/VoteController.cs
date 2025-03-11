using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VotingSystem.API.DTO.Vote;
using VotingSystem.API.Services;

[Route("api/[controller]")]
[ApiController]
public class VoteController : ControllerBase
{
    private readonly IVoteService _voteService;

    public VoteController(IVoteService voteService)
    {
        _voteService = voteService;
    }
    [HttpPost("cast")]
    public async Task<IActionResult> CastVote([FromBody] VoteRequestDTO voteDto)
    {
        if (voteDto == null)
            return BadRequest("Invalid vote request.");

        try
        {
            bool isVoteCast = await _voteService.CastVoteAsync(voteDto);

            if (isVoteCast)
            {
                return Ok(new
                {
                    Message = voteDto.IsAbstained
                        ? "You have successfully abstained from voting."
                        : "Vote cast successfully."
                });
            }

            return BadRequest("Vote could not be cast. Please try again.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
        }
    }

    [HttpGet("candidates/{voterCardNumber}")]
    public async Task<IActionResult> GetCandidatesByVoterState(string voterCardNumber)
    {
        if (string.IsNullOrEmpty(voterCardNumber))
            return BadRequest("Voter card number is required.");

        try
        {
            var candidates = await _voteService.GetCandidatesByVoterStateAsync(voterCardNumber);
            return Ok(candidates);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("total-votes")]
    public async Task<IActionResult> GetTotalVotes()
    {
        try
        {
            var totalVotes = await _voteService.GetTotalVotesAsync();
            return Ok(new { TotalVotes = totalVotes });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("votes-by-state/{stateId}")]
    public async Task<IActionResult> GetVotesByState(int stateId)
    {
        try
        {
            var stateVotes = await _voteService.GetVotesByStateAsync(stateId);
            return Ok(new { StateId = stateId, Votes = stateVotes });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
