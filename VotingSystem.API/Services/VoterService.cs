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
                                                                                                                                                                                                                                                                                        
    public async Task<VoterResponseDTO> RegisterVoterAsync(VoterRequestDTO voterRequest)
    {
 
        if (voterRequest.Age < 18)
        {
            throw new ArgumentException("Voter must be at least 18 years old.");
        }

        string uniqueVoterCardNumber =  GenerateRandomVoterCard();

        var existingVoter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == uniqueVoterCardNumber);
        if (existingVoter != null)
        {
            throw new ArgumentException("A voter with the same Voter Card Number already exists.");
        }
        string voterCardNumber = GenerateRandomVoterCard();

        var voter = new Voter
        {
            VoterName = voterRequest.VoterName,
            Age = voterRequest.Age,
            StateId = voterRequest.StateId,
            VoterCardNumber = voterCardNumber
        };

        _context.Voters.Add(voter);
        await _context.SaveChangesAsync();

        return new VoterResponseDTO
        {
            VoterId = voter.VoterId,
            VoterName = voter.VoterName,
            VoterCardNumber = voter.VoterCardNumber
        };
    }

    public async Task<IEnumerable<VoterResponseDTO>> GetAllVotersAsync()
    {
        var voters = await _context.Voters.ToListAsync();
        return voters.Select(v => new VoterResponseDTO
        {
            VoterId = v.VoterId,
            VoterName = v.VoterName,
            VoterCardNumber = v.VoterCardNumber,
            Age=v.Age,
            StateId=v.StateId
        });
    }

    // ✅ Get Voter by ID (Admin Only)
    //public async Task<VoterResponseDTO> GetVoterByCardNumberAsync(string voterCardNumber)
    //{
    //    var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);

    //    if (voter == null)
    //    {
    //        throw new ArgumentException("Voter not found.");
    //    }

    //    return _mapper.Map<VoterResponseDTO>(voter);
    //}
    public async Task<VoterResponseDTO> GetVoterByCardNumberAsync(string voterCardNumber)
    {
        var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);

        if (voter == null)
        {
            throw new ArgumentException("Voter not found.");
        }

        // Manually map Voter to VoterResponseDTO
        var voterDTO = new VoterResponseDTO
        {
            VoterId = voter.VoterId,
            VoterName = voter.VoterName,
            Age = voter.Age,
            StateId = voter.StateId,
            VoterCardNumber = voter.VoterCardNumber
        };

        return voterDTO;
    }

    //public async Task<VoterResponseDTO> GetVoterByIdAsync(Guid voterId)
    //{
    //    var voter = await _context.Voters.FindAsync(voterId);
    //    //var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterId == voterId);

    //    if (voter == null)
    //        return null;

    //    return new VoterResponseDTO
    //    {
    //        VoterId = voter.VoterId,
    //        VoterName = voter.VoterName,
    //        VoterCardNumber = voter.VoterCardNumber
    //    };
    //}

    // ✅ Update Voter (Admin Only)
    //public async Task<VoterResponseDTO> UpdateVoterAsync(string voterCardNumber, VoterRequestDTO voterDto)
    //{
    //    var voter = await _context.Voters.FindAsync(voterCardNumber);
    //    if (voter == null)
    //        return null;

    //    voter.VoterName = voterDto.VoterName;
    //    voter.Age = voterDto.Age;
    //    voter.StateId = voterDto.StateId;

    //    await _context.SaveChangesAsync();

    //    return new VoterResponseDTO
    //    {
    //        VoterId = voter.VoterId,
    //        VoterName = voter.VoterName,
    //        VoterCardNumber = voter.VoterCardNumber,
    //        Age = voter.Age,
    //        StateId=voter.StateId
    //    };
    //}
    public async Task<VoterResponseDTO> UpdateVoterAsync(string voterCardNumber, VoterRequestDTO voterDto)
    {
        var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);
        if (voter == null)
            return null;

        voter.VoterName = voterDto.VoterName;
        voter.Age = voterDto.Age;
        voter.StateId = voterDto.StateId;

        await _context.SaveChangesAsync();

        return new VoterResponseDTO
        {
            VoterId=voter.VoterId,
            VoterName = voter.VoterName,
            VoterCardNumber = voter.VoterCardNumber,
            Age = voter.Age,
            StateId = voter.StateId
        };
    }

    public async Task<IEnumerable<VoterResponseDTO>> GetVotersByStateIdAsync(int stateId)
    {
        var voters = await _context.Voters.Where(v => v.StateId == stateId).ToListAsync();
        return voters.Select(v => new VoterResponseDTO
        {
            VoterId = v.VoterId,
            VoterName = v.VoterName,
            VoterCardNumber = v.VoterCardNumber,
            Age=v.Age,
            StateId=v.StateId
        });
    }

    private string GenerateRandomVoterCard()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
