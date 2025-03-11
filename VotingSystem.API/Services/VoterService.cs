using Microsoft.EntityFrameworkCore;
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

    public VoterService(VotingDbContext context)
    {
        _context = context;
    }

    // ✅ Register a Voter (Generate 6-digit Voter Card Number)
    public async Task<VoterResponseDTO> RegisterVoterAsync(VoterRequestDTO voterRequest)
    {
        // ✅ Check if the voter is under 18
        if (voterRequest.Age < 18)
        {
            throw new ArgumentException("Voter must be at least 18 years old.");
        }

        // ✅ Check if the voter already exists by name (or another unique field)
        string uniqueVoterCardNumber =  GenerateRandomVoterCard();

        var existingVoter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == uniqueVoterCardNumber);
        if (existingVoter != null)
        {
            throw new ArgumentException("A voter with the same Voter Card Number already exists.");
        }
        // ✅ Generate a random voter card number (6 digits)
        string voterCardNumber = GenerateRandomVoterCard();

        // ✅ Create a new Voter entity
        var voter = new Voter
        {
            VoterName = voterRequest.VoterName,
            Age = voterRequest.Age,
            StateId = voterRequest.StateId,
            VoterCardNumber = voterCardNumber
        };

        // ✅ Save to database
        _context.Voters.Add(voter);
        await _context.SaveChangesAsync();

        // ✅ Return response DTO
        return new VoterResponseDTO
        {
            VoterId = voter.VoterId,
            VoterName = voter.VoterName,
            VoterCardNumber = voter.VoterCardNumber
        };
    }


    // ✅ Get All Voters (Admin Only)
    public async Task<IEnumerable<VoterResponseDTO>> GetAllVotersAsync()
    {
        var voters = await _context.Voters.ToListAsync();
        return voters.Select(v => new VoterResponseDTO
        {
            VoterId = v.VoterId,
            VoterName = v.VoterName,
            VoterCardNumber = v.VoterCardNumber
        });
    }

    // ✅ Get Voter by ID (Admin Only)
    public async Task<VoterResponseDTO> GetVoterByIdAsync(Guid voterId)
    {
        var voter = await _context.Voters.FindAsync(voterId);
        //var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterId == voterId);

        if (voter == null)
            return null;

        return new VoterResponseDTO
        {
            VoterId = voter.VoterId,
            VoterName = voter.VoterName,
            VoterCardNumber = voter.VoterCardNumber
        };
    }

    // ✅ Update Voter (Admin Only)
    public async Task<VoterResponseDTO> UpdateVoterAsync(int voterId, VoterRequestDTO voterDto)
    {
        var voter = await _context.Voters.FindAsync(voterId);
        if (voter == null)
            return null;

        voter.VoterName = voterDto.VoterName;
        voter.Age = voterDto.Age;
        voter.StateId = voterDto.StateId;

        await _context.SaveChangesAsync();

        return new VoterResponseDTO
        {
            VoterId = voter.VoterId,
            VoterName = voter.VoterName,
            VoterCardNumber = voter.VoterCardNumber
        };
    }

    // ✅ Get Voters by State ID (Admin Only)
    public async Task<IEnumerable<VoterResponseDTO>> GetVotersByStateIdAsync(int stateId)
    {
        var voters = await _context.Voters.Where(v => v.StateId == stateId).ToListAsync();
        return voters.Select(v => new VoterResponseDTO
        {
            VoterId = v.VoterId,
            VoterName = v.VoterName,
            VoterCardNumber = v.VoterCardNumber
        });
    }

    // 🔹 Helper Method: Generate Random 6-digit Voter Card Number
    private string GenerateRandomVoterCard()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
