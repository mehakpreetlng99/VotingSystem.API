﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCandidate([FromBody] CandidateRequestDTO candidateDto)
        {
            try
            {
                var createdCandidate = await _candidateService.CreateCandidateAsync(candidateDto);
                return CreatedAtAction(nameof(GetCandidateById), new { id = createdCandidate.CandidateId }, createdCandidate);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCandidate(int id, [FromBody] CandidateRequestDTO candidateDto)
        {
            try
            {
                var updatedCandidate = await _candidateService.UpdateCandidateAsync(id, candidateDto);
                return Ok(updatedCandidate);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Candidate not found." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var isDeleted = await _candidateService.DeleteCandidateAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = "Candidate not found." });
            }
            return Ok(new { message = "Candidate removed successfully." });
        }

        [HttpGet("allCandidates")]
        public async Task<IActionResult> GetAllCandidates()
        {
            var candidates = await _candidateService.GetAllCandidatesAsync();
            return Ok(new { data = candidates });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCandidateById(int id)
        {
            var candidate = await _candidateService.GetCandidateByIdAsync(id);
            if (candidate == null)
            {
                return NotFound(new { message = "Candidate not found." });
            }
            return Ok(candidate);
        }


    }
}
