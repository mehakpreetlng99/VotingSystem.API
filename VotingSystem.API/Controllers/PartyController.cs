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
    [Authorize(Roles = "Admin")] // Restrict all endpoints to Admins
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;
        private readonly ILogger<PartyController> _logger;

        public PartyController(IPartyService partyService, ILogger<PartyController> logger)
        {
            _partyService = partyService;
            _logger = logger;
        }

        // Create a new party
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

        // Get all parties
        [HttpGet]
        [AllowAnonymous] // Allow all users to view parties
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

        // Get party by ID
        [HttpGet("{id}")]
        [AllowAnonymous] // Allow all users to view specific parties
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

        // Delete party by ID
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

        // Update party by ID
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

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using VotingSystem.API.Models;
//using VotingSystem.API.Services;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using VotingSystem.API.DTO.Party;

//namespace VotingSystem.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(Roles = "Admin")] // Restrict all endpoints to Admins
//    public class PartyController : ControllerBase
//    {
//        private readonly IPartyService _partyService;

//        public PartyController(IPartyService partyService)
//        {
//            _partyService = partyService;
//        }

//        // Create a new party
//        [HttpPost]
//        public async Task<ActionResult<PartyResponseDTO>> CreateParty([FromBody] PartyRequestDTO partyDto)
//        {
//            try
//            {
//                var createdParty = await _partyService.CreatePartyAsync(partyDto);
//                return CreatedAtAction(nameof(GetPartyById), new { id = createdParty.PartyId }, createdParty);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { Message = ex.Message });
//            }
//        }

//        // Get all parties
//        [HttpGet]
//        [AllowAnonymous] // Allow all users to view parties
//        public async Task<ActionResult<IEnumerable<PartyResponseDTO>>> GetAllParties()
//        {
//            try
//            {
//                var parties = await _partyService.GetAllPartiesAsync();
//                return Ok(parties);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { Message = ex.Message });
//            }
//        }

//        // Get party by ID
//        [HttpGet("{id}")]
//        [AllowAnonymous] // Allow all users to view specific parties
//        public async Task<ActionResult<PartyResponseDTO>> GetPartyById(int id)
//        {
//            try
//            {
//                var party = await _partyService.GetPartyByIdAsync(id);
//                if (party == null)
//                {
//                    return NotFound();
//                }
//                return Ok(party);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { Message = ex.Message });
//            }
//        }

//        // Delete party by ID
//        [HttpDelete("{id}")]
//        public async Task<ActionResult> DeleteParty(int id)
//        {
//            try
//            {
//                var result = await _partyService.DeletePartyAsync(id);
//                if (!result)
//                {
//                    return NotFound();
//                }
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { Message = ex.Message });
//            }
//        }

//        // Update party by ID
//        [HttpPut("{id}")]
//        public async Task<ActionResult<PartyResponseDTO>> UpdateParty(int id, [FromBody] PartyRequestDTO partyDto)
//        {
//            try
//            {
//                var updatedParty = await _partyService.UpdatePartyAsync(id, partyDto);
//                if (updatedParty == null)
//                {
//                    return NotFound();
//                }
//                return Ok(updatedParty);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { Message = ex.Message });
//            }
//        }
//    }
//}

