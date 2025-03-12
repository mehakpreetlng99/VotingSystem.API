
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VotingSystem.API.DTO.State;
using VotingSystem.API.Models;
using VotingSystem.API.Services;
using VotingSystem.API.Services.Interface;

namespace VotingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly ILogger<StateController> _logger;

        public StateController(IStateService stateService, ILogger<StateController> logger)
        {
            _stateService = stateService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<StateResponseDTO>> CreateState([FromBody] StateRequestDTO stateDto)
        {
            try
            {
                var createdState = await _stateService.CreateStateAsync(stateDto);
                return CreatedAtAction(nameof(GetStateById), new { id = createdState.StateId }, createdState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating state.");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateResponseDTO>>> GetAllStates()
        {
            try
            {
                var states = await _stateService.GetAllStatesAsync();
                return Ok(states);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all states.");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StateResponseDTO>> GetStateById(int id)
        {
            try
            {
                var state = await _stateService.GetStateByIdAsync(id);
                if (state == null)
                {
                    return NotFound();
                }
                return Ok(state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching state by ID: {Id}", id);
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteState(int id)
        {
            try
            {
                var result = await _stateService.DeleteStateAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting state with ID: {Id}", id);
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StateResponseDTO>> UpdateState(int id, [FromBody] StateRequestDTO stateDto)
        {
            try
            {
                var updatedState = await _stateService.UpdateStateAsync(id, stateDto);
                if (updatedState == null)
                {
                    return NotFound();
                }
                return Ok(updatedState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating state with ID: {Id}", id);
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
