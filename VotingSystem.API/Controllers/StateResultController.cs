using Microsoft.AspNetCore.Mvc;
using VotingSystem.API.DTO;
using VotingSystem.API.Services;
using System;
using System.Threading.Tasks;
using VotingSystem.API.Models;
using VotingSystem.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;

namespace VotingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateResultController:ControllerBase
    {
        private readonly IStateResultService _stateResultService;
        public StateResultController(IStateResultService stateResultService)
        {
            _stateResultService = stateResultService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("declarewinner/{stateId}")]
        public async Task<ActionResult<string>> DeclareWinner(int stateId)
        {
            try
            {
                var result = await _stateResultService.DeclareWinnerAsync(stateId);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // Get the state result for a specific state
        [HttpGet("{stateId}")]
        public async Task<ActionResult<StateResult>> GetStateResult(int stateId)
        {
            try
            {
                var stateResult = await _stateResultService.GetStateResultByStateIdAsync(stateId);
                return Ok(stateResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
