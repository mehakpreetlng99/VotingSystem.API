﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using VotingSystem.API.DTO.Candidate;
using System;

namespace VotingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        private readonly ILogger<CandidateController> _logger;

        public CandidateController(ICandidateService candidateService, ILogger<CandidateController> logger)
        {
            _candidateService = candidateService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCandidate([FromBody] CandidateRequestDTO candidateDto)
        {
            try
            {
                _logger.LogInformation("Creating a new candidate.");
                var createdCandidate = await _candidateService.CreateCandidateAsync(candidateDto);
                return CreatedAtAction(nameof(GetCandidateById), new { id = createdCandidate.CandidateId }, createdCandidate);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error occurred while creating a candidate.");
                return BadRequest(new { message = ex.Message.Split('\n')[0] }); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating a candidate.");
                return StatusCode(500, new { message = ex.Message.Split('\n')[0] }); 
            }
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCandidate(int id, [FromBody] CandidateRequestDTO candidateDto)
        {
            try
            {
                _logger.LogInformation($"Updating candidate with ID: {id}");
                var updatedCandidate = await _candidateService.UpdateCandidateAsync(id, candidateDto);
                return Ok(updatedCandidate);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Candidate with ID {id} not found for update.");
                return NotFound(new { message = "Candidate not found." });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Error occurred while updating candidate with ID: {id}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            _logger.LogInformation($"Attempting to delete candidate with ID: {id}");

            var resultMessage = await _candidateService.DeleteCandidateAsync(id);

            if (resultMessage == "Candidate not found.")
            {
                return NotFound(new { message = resultMessage });
            }
            else if (resultMessage == "Cannot delete candidate with existing votes.")
            {
                return BadRequest(new { message = resultMessage });
            }

            _logger.LogInformation($"Candidate with ID {id} deleted successfully.");
            return Ok(new { message = resultMessage });
        }


        [HttpGet("allCandidates")]
        public async Task<IActionResult> GetAllCandidates()
        {
            _logger.LogInformation("Fetching all candidates.");
            var candidates = await _candidateService.GetAllCandidatesAsync();
            return Ok(new { data = candidates });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCandidateById(int id)
        {
            _logger.LogInformation($"Fetching candidate with ID: {id}");
            var candidate = await _candidateService.GetCandidateByIdAsync(id);
            if (candidate == null)
            {
                _logger.LogWarning($"Candidate with ID {id} not found.");
                return NotFound(new { message = "Candidate not found." });
            }
            return Ok(candidate);
        }
    }
}

