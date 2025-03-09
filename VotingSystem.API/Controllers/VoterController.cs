using Microsoft.AspNetCore.Mvc;
using VotingSystem.API.Services;
using VotingSystem.API.Models ;
namespace VotingSystem.API.Controllers
{
    [ApiController]
    [Route("api/voters")]
    public class VoterController:ControllerBase
    {
        private readonly IVoterService _voterService;
        public VoterController(IVoterService voterService)
        {
            _voterService = voterService;
        }

        // GET: api/voters

        [HttpGet]
        public async Task <IActionResult> GetAllVoters()
        {
            var voters = await _voterService.AllVotersAsync();
            return Ok(voters);
        }

        //GEt: api/voters/{id}
        [HttpGet("{id}")]
        public async Task <IActionResult> GetVoterById(Guid id)
        {
            var voter = await _voterService.GetVoterByIdAsync(id);
            if (voter == null) return NotFound("Voter not Found");
            return Ok(voter);
        }

        //POST: api/voters/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterVoter([FromBody] Voter voter)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _voterService.RegisterVoterAsync(voter);
            if (!result) return BadRequest("Voter Registration Failed.'Duplicate Card Number?");
            return Ok("Voter Registered successfully");
        }

        //POST :api/voters/castvote
        [HttpPost("castvote")]
        public async Task<IActionResult> CastVote([FromQuery] Guid voterId, [FromQuery] int candidateId)
        {
            var result = await _voterService.CastVoteAsync(voterId, candidateId);
            if (!result) return BadRequest("Failed to cast vote. Voter might have already voted.");
            return Ok("Vote cast successfully");
        }
    }
}
