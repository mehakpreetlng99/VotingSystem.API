
using VotingSystem.API.Data;
using VotingSystem.API.DTO.Candidate;
using VotingSystem.API.DTO.Vote;
using VotingSystem.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.API.Services;

public class VoteService : IVoteService
{
    private readonly VotingDbContext _context;
    private readonly ILogger<VoteService> _logger;

    public VoteService(VotingDbContext context, ILogger<VoteService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CastVoteAsync(VoteRequestDTO voteDto)
    {
        try
        {
            _logger.LogInformation("Attempting to cast vote for voter: {VoterCardNumber}", voteDto.VoterCardNumber);

            var voter = await _context.Voters
                .Where(v => v.VoterCardNumber == voteDto.VoterCardNumber)
                .FirstOrDefaultAsync();

            if (voter == null)
            {
                _logger.LogWarning("Voter not found: {VoterCardNumber}", voteDto.VoterCardNumber);
                throw new KeyNotFoundException("Voter not found.");
            }

            if (_context.Votes.Any(v => v.VoterId == voter.VoterId))
            {
                _logger.LogWarning("Voter {VoterCardNumber} has already voted.", voteDto.VoterCardNumber);
                throw new InvalidOperationException("Voter has already cast a vote.");
            }

            Vote vote;
            if (voteDto.IsAbstained)
            {
                _logger.LogInformation("Voter {VoterCardNumber} abstained from voting.", voteDto.VoterCardNumber);

                vote = new Vote
                {
                    VoterId = voter.VoterId,
                    CandidateId = null,
                    StateId = voter.StateId,
                    VoteTime = DateTime.UtcNow
                };
            }
            else
            {
                if (voteDto.CandidateId == null)
                {
                    throw new InvalidOperationException("Candidate ID is required unless abstaining.");
                }

                var candidate = await _context.Candidates
                    .Where(c => c.CandidateId == voteDto.CandidateId)
                    .FirstOrDefaultAsync();

                if (candidate == null)
                {
                    _logger.LogWarning("Candidate not found: {CandidateId}", voteDto.CandidateId);
                    throw new KeyNotFoundException("Candidate not found.");
                }

                if (candidate.StateId != voter.StateId)
                {
                    _logger.LogWarning("Voter {VoterCardNumber} attempted to vote outside their state.", voteDto.VoterCardNumber);
                    throw new UnauthorizedAccessException("You can only vote for candidates in your own state.");
                }

                vote = new Vote
                {
                    VoterId = voter.VoterId,
                    CandidateId = candidate.CandidateId,
                    StateId = voter.StateId,
                    VoteTime = DateTime.UtcNow
                };

                candidate.VoteCount += 1;
            }

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Vote successfully cast by {VoterCardNumber}", voteDto.VoterCardNumber);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while casting vote for voter: {VoterCardNumber}", voteDto.VoterCardNumber);
            throw;
        }
    }

    public async Task<List<CandidateResponseDTO>> GetCandidatesByVoterStateAsync(string voterCardNumber)
    {
        try
        {
            _logger.LogInformation("Fetching candidates for voter: {VoterCardNumber}", voterCardNumber);

            var voter = await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);
            if (voter == null)
            {
                _logger.LogWarning("Voter not found: {VoterCardNumber}", voterCardNumber);
                throw new KeyNotFoundException("Voter not found.");
            }

            return await _context.Candidates
                .Where(c => c.StateId == voter.StateId)
                .Select(c => new CandidateResponseDTO
                {
                    CandidateId = c.CandidateId,
                    CandidateName = c.CandidateName,
                    PartyName = _context.Parties.Where(p => p.PartyId == c.PartyId).Select(p => p.PartyName).FirstOrDefault()
                }).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching candidates for voter: {VoterCardNumber}", voterCardNumber);
            throw;
        }
    }

    public async Task<int> GetTotalVotesAsync()
    {
        try
        {
            _logger.LogInformation("Fetching total vote count.");
            return await _context.Votes.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting total votes.");
            throw;
        }
    }

    public async Task<int> GetVotesByStateAsync(int stateId)
    {
        try
        {
            _logger.LogInformation("Fetching total votes for state {StateId}", stateId);
            return await _context.Votes.CountAsync(v => v.StateId == stateId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting votes for state {StateId}", stateId);
            throw;
        }
    }
}

//using VotingSystem.API.Data;
//using VotingSystem.API.DTO.Candidate;
//using VotingSystem.API.DTO.Vote;
//using VotingSystem.API.Models;
//using VotingSystem.API.Services;
//using Microsoft.EntityFrameworkCore;

//public class VoteService : IVoteService
//{
//    private readonly VotingDbContext _context;

//    public VoteService(VotingDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<bool> CastVoteAsync(VoteRequestDTO voteDto)
//    {
//        var voter = await _context.Voters
//            .Where(v => v.VoterCardNumber == voteDto.VoterCardNumber)
//            .FirstOrDefaultAsync();

//        if (voter == null)
//            throw new KeyNotFoundException("Voter not found.");

//        if (_context.Votes.Any(v => v.VoterId == voter.VoterId))
//            throw new InvalidOperationException("Voter has already cast a vote.");

//        if (voteDto.IsAbstained)
//        { 
//            var abstainVote = new Vote
//            {
//                VoterId = voter.VoterId,
//                CandidateId = null,  
//                StateId = voter.StateId,
//                VoteTime = DateTime.UtcNow
//            };

//            _context.Votes.Add(abstainVote);
//        }
//        else
//        {
//            if (voteDto.CandidateId == null)
//                throw new InvalidOperationException("Candidate ID is required unless abstaining.");

//            var candidate = await _context.Candidates
//                .Where(c => c.CandidateId == voteDto.CandidateId)
//                .FirstOrDefaultAsync();

//            if (candidate == null)
//                throw new KeyNotFoundException("Candidate not found.");

//            if (candidate.StateId != voter.StateId)
//                throw new UnauthorizedAccessException("You can only vote for candidates in your own state.");

//            var vote = new Vote
//            {
//                VoterId = voter.VoterId,
//                CandidateId = candidate.CandidateId,
//                StateId = voter.StateId,
//                VoteTime = DateTime.UtcNow
//            };

//            _context.Votes.Add(vote);

//            candidate.VoteCount += 1;  
//        }

//        await _context.SaveChangesAsync();
//        return true;
//    }

//    public async Task<List<CandidateResponseDTO>> GetCandidatesByVoterStateAsync(string voterCardNumber)
//    {
//        try
//        {
//            var voter = await _context.Voters
//                .FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);

//            if (voter == null)
//                throw new KeyNotFoundException("Voter not found.");

//            return await _context.Candidates
//                .Where(c => c.StateId == voter.StateId)
//                .Select(c => new CandidateResponseDTO
//                {
//                    CandidateId = c.CandidateId,
//                    CandidateName = c.CandidateName,
//                    PartyName = _context.Parties.Where(p => p.PartyId == c.PartyId).Select(p => p.PartyName).FirstOrDefault()
//                }).ToListAsync();
//        }
//        catch (Exception ex)
//        {
//            throw new Exception($"Error fetching candidates: {ex.Message}");
//        }
//    }

//    public async Task<int> GetTotalVotesAsync()
//    {
//        try
//        {
//            return await _context.Votes.CountAsync();
//        }
//        catch (Exception ex)
//        {
//            throw new Exception($"Error counting total votes: {ex.Message}");
//        }
//    }

//    public async Task<int> GetVotesByStateAsync(int stateId)
//    {
//        try
//        {
//            return await _context.Votes.CountAsync(v => v.StateId == stateId);
//        }
//        catch (Exception ex)
//        {
//            throw new Exception($"Error counting votes for state {stateId}: {ex.Message}");
//        }
//    }
//}
