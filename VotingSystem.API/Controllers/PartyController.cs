using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Models;
using VotingSystem.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.API.DTO.Party;

namespace VotingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;
        private readonly ILogger<PartyController> _logger;

        public PartyController(IPartyService partyService, ILogger<PartyController> logger)
        {
            _partyService = partyService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<PartyResponseDTO>> CreateParty([FromBody] PartyRequestDTO partyDto)
        {
            try
            {
                var createdParty = await _partyService.CreatePartyAsync(partyDto);
                return CreatedAtAction(nameof(GetPartyById), new { id = createdParty.PartyId }, createdParty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating party");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous] 
        public async Task<ActionResult<IEnumerable<PartyResponseDTO>>> GetAllParties()
        {
            try
            {
                var parties = await _partyService.GetAllPartiesAsync();
                return Ok(parties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all parties");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous] 
        public async Task<ActionResult<PartyResponseDTO>> GetPartyById(int id)
        {
            try
            {
                var party = await _partyService.GetPartyByIdAsync(id);
                if (party == null)
                {
                    return NotFound();
                }
                return Ok(party);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving party with ID {PartyId}", id);
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteParty(int id)
        {
            try
            {
                var result = await _partyService.DeletePartyAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting party with ID {PartyId}", id);
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PartyResponseDTO>> UpdateParty(int id, [FromBody] PartyRequestDTO partyDto)
        {
            try
            {
                var updatedParty = await _partyService.UpdatePartyAsync(id, partyDto);
                if (updatedParty == null)
                {
                    return NotFound();
                }
                return Ok(updatedParty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating party with ID {PartyId}", id);
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}


