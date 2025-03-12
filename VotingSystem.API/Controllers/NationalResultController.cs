using Microsoft.AspNetCore.Mvc;
using VotingSystem.API.DTO.NationalResult;
using VotingSystem.API.Services.Interface;

namespace VotingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalResultController : Controller
    {
        private readonly INationalResultService _nationalResultService;
        public NationalResultController(INationalResultService nationalResultService)
        {
            _nationalResultService = nationalResultService;
        }
        [HttpGet("national-result")]
        public async Task<ActionResult<NationalResultResponseDTO>> GetNationalResult()
        {
            var result = await _nationalResultService.GetNationalResultAsync();
            if (result == null)
            {
                return NotFound(new { Message = "National result not found." });
            }
            return Ok(result);
        }

    }
}
