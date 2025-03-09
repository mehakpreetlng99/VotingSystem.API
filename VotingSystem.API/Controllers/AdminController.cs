using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
namespace VotingSystem.API.Controllers
{
    

[Route("api/admin")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]  // Only Admin can access
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult GetAdminDashboard()
        {
            return Ok("Welcome Admin! This is your dashboard.");
        }
    }

}
