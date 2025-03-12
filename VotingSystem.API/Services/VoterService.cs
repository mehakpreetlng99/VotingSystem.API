
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Data;
using VotingSystem.API.DTO.Voter;
using VotingSystem.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class VoterService : IVoterService
{
    private readonly VotingDbContext _context;
    private readonly ILogger<VoterService> _logger;

    public VoterService(VotingDbContext context, ILogger<VoterService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<VoterResponseDTO> RegisterVoterAsync(VoterRequestDTO voterRequest)
    {
        if (voterRequest.Age < 18)
        {
            _logger.LogWarning("Voter registration failed: Age {Age} is below 18.", voterRequest.Age);
            throw new ArgumentException("Voter must be at least 18 years old.");
        }

        string voterCardNumber = GenerateRandomVoterCard();
        var existingVoter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);
        if (existingVoter != null)
        {
            _logger.LogWarning("Duplicate VoterCardNumber generated: {VoterCardNumber}", voterCardNumber);
            throw new ArgumentException("A voter with the same Voter Card Number already exists.");
        }

        var voter = new Voter
        {
            VoterName = voterRequest.VoterName,
            Age = voterRequest.Age,
            StateId = voterRequest.StateId,
            VoterCardNumber = voterCardNumber
        };

        _context.Voters.Add(voter);
        await _context.SaveChangesAsync();
        _logger.LogInformation("New voter registered: {VoterName}, VoterCardNumber: {VoterCardNumber}", voter.VoterName, voter.VoterCardNumber);

        return new VoterResponseDTO
        {
            VoterId = voter.VoterId,
            VoterName = voter.VoterName,
            VoterCardNumber = voter.VoterCardNumber,
            StateId = voter.StateId,
            Age = voter.Age
        };
    }

    public async Task<VoterResponseDTO> GetVoterByCardNumberAsync(string voterCardNumber)
    {
        var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);

        if (voter == null)
        {
            _logger.LogWarning("Voter lookup failed: No voter found for VoterCardNumber {VoterCardNumber}", voterCardNumber);
            throw new ArgumentException("Voter not found.");
        }

        _logger.LogInformation("Voter retrieved: {VoterName}, VoterCardNumber: {VoterCardNumber}", voter.VoterName, voter.VoterCardNumber);
        return new VoterResponseDTO
        {
            VoterId = voter.VoterId,
            VoterName = voter.VoterName,
            Age = voter.Age,
            StateId = voter.StateId,
            VoterCardNumber = voter.VoterCardNumber
        };
    }

    public async Task<VoterResponseDTO> UpdateVoterAsync(string voterCardNumber, VoterRequestDTO voterDto)
    {
        var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);
        if (voter == null)
        {
            _logger.LogWarning("Voter update failed: No voter found for VoterCardNumber {VoterCardNumber}", voterCardNumber);
            return null;
        }

        voter.VoterName = voterDto.VoterName;
        voter.Age = voterDto.Age;
        voter.StateId = voterDto.StateId;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Voter updated: {VoterName}, VoterCardNumber: {VoterCardNumber}", voter.VoterName, voter.VoterCardNumber);

        return new VoterResponseDTO
        {
            VoterId = voter.VoterId,
            VoterName = voter.VoterName,
            VoterCardNumber = voter.VoterCardNumber,
            Age = voter.Age,
            StateId = voter.StateId
        };
    }

    public async Task<IEnumerable<VoterResponseDTO>> GetAllVotersAsync()
    {
        var voters = await _context.Voters.ToListAsync();
        _logger.LogInformation("Retrieved all voters. Total count: {Count}", voters.Count);
        return voters.Select(v => new VoterResponseDTO
        {
            VoterId = v.VoterId,
            VoterName = v.VoterName,
            VoterCardNumber = v.VoterCardNumber,
            Age = v.Age,
            StateId = v.StateId
        });
    }

    public async Task<IEnumerable<VoterResponseDTO>> GetVotersByStateIdAsync(int stateId)
    {
        var voters = await _context.Voters.Where(v => v.StateId == stateId).ToListAsync();
        _logger.LogInformation("Retrieved voters from StateId {StateId}. Total count: {Count}", stateId, voters.Count);
        return voters.Select(v => new VoterResponseDTO
        {
            VoterId = v.VoterId,
            VoterName = v.VoterName,
            VoterCardNumber = v.VoterCardNumber,
            Age = v.Age,
            StateId = v.StateId
        });
    }

    private string GenerateRandomVoterCard()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}

