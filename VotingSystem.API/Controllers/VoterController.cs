using Microsoft.AspNetCore.Mvc;
using VotingSystem.API.Services;
using VotingSystem.API.Models ;
using Microsoft.AspNetCore.Authorization;
namespace VotingSystem.API.Controllers
{
    [ApiController]
    [Route("api/voters")]
    [Authorize(Policy = "VoterPolicy")]
    public class VoterController:ControllerBase
    {
        private readonly IVoterService _voterService;
        public VoterController(IVoterService voterService)
        {
            _voterService = voterService;
        }

        [HttpGet("profile")]
        public IActionResult GetVoterProfile()
        {
            return Ok("Welcome Voter! This is your profile.");
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
            bool result = await _voterService.RegisterVoterAsync(voter);
            if (result) 
                return Ok("Voter Registered successfully");
            else
              return BadRequest("Voter Registration Failed.'Duplicate Card Number?");
            
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
