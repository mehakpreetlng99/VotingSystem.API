using Microsoft.EntityFrameworkCore;
using VotingSystem.API.Data;
using VotingSystem.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VotingSystem.API.Services
{

    public class VoterService : IVoterService
    {
        private readonly VotingDbContext _context;
        private readonly ILogger<VoterService> _logger;

        public VoterService(VotingDbContext context, ILogger<VoterService> logger)
        {
            _context = context;
            _logger = logger;
        }

        //  Get All Voters
        public async Task<IEnumerable<Voter>> AllVotersAsync()
        {
            return await _context.Voters.ToListAsync();
        }

        //  Get Voter by ID
        public async Task<Voter?> GetVoterByIdAsync(Guid voterId)
        {
            return await _context.Voters.FindAsync(voterId);
        }

        //  Register Voter
        public async Task<bool> RegisterVoterAsync(Voter voter)
        {
            if (await _context.Voters.AnyAsync(v => v.VoterCardNumber == voter.VoterCardNumber))
            {
                _logger.LogWarning("Voter with card number {VoterCardNumber} already exists.", voter.VoterCardNumber);
                return false; // Prevent duplicate voter registration
            }

            await _context.Voters.AddAsync(voter);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Voter?> GetVoterByCardNumberAsync(string voterCardNumber)
        {
            return await _context.Voters.FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);
        }
        //  Cast Vote
        public async Task<bool> CastVoteAsync(Guid voterId, int candidateId)
        {
            var voter = await _context.Voters.FindAsync(voterId);
            if (voter == null) return false; // Voter not found

            if (await _context.Votes.AnyAsync(v => v.VoterId == voterId))
            {
                _logger.LogWarning("Voter {VoterId} has already voted.", voterId);
                return false; // Prevent multiple voting
            }

            var vote = new Vote { VoterId = voterId, CandidateId = candidateId, VoteTime = DateTime.UtcNow };
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
            return true;
        }
    }


}
