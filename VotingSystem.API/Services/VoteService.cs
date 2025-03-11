using VotingSystem.API.Data;
using VotingSystem.API.DTO.Candidate;
using VotingSystem.API.DTO.Vote;
using VotingSystem.API.Models;
using VotingSystem.API.Services;
using Microsoft.EntityFrameworkCore;

public class VoteService : IVoteService
{
    private readonly VotingDbContext _context;

    public VoteService(VotingDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CastVoteAsync(VoteRequestDTO voteDto)
    {
        var voter = await _context.Voters
            .Where(v => v.VoterCardNumber == voteDto.VoterCardNumber)
            .FirstOrDefaultAsync();

        if (voter == null)
            throw new KeyNotFoundException("Voter not found.");

        if (_context.Votes.Any(v => v.VoterId == voter.VoterId))
            throw new InvalidOperationException("Voter has already cast a vote.");

        if (voteDto.IsAbstained)
        {
            // Abstention vote
            var abstainVote = new Vote
            {
                VoterId = voter.VoterId,
                CandidateId = null,  // No candidate selected
                StateId = voter.StateId,
                VoteTime = DateTime.UtcNow
            };

            _context.Votes.Add(abstainVote);
        }
        else
        {
            if (voteDto.CandidateId == null)
                throw new InvalidOperationException("Candidate ID is required unless abstaining.");

            var candidate = await _context.Candidates
                .Where(c => c.CandidateId == voteDto.CandidateId)
                .FirstOrDefaultAsync();

            if (candidate == null)
                throw new KeyNotFoundException("Candidate not found.");

            if (candidate.StateId != voter.StateId)
                throw new UnauthorizedAccessException("You can only vote for candidates in your own state.");

            var vote = new Vote
            {
                VoterId = voter.VoterId,
                CandidateId = candidate.CandidateId,
                StateId = voter.StateId,
                VoteTime = DateTime.UtcNow
            };

            _context.Votes.Add(vote);

            candidate.VoteCount += 1;  // Increment the vote count for the candidate
        }

        await _context.SaveChangesAsync();
        return true;
    }

    //public async Task<bool> CastVoteAsync(VoteRequestDTO voteDto)
    //{
    //    try
    //    {
    //        var voter = await _context.Voters
    //            .FirstOrDefaultAsync(v => v.VoterCardNumber == voteDto.VoterCardNumber);

    //        if (voter == null)
    //            throw new KeyNotFoundException("Voter not found.");

    //        var currentTime = DateTime.UtcNow.TimeOfDay;
    //        if (currentTime < new TimeSpan(8, 0, 0) || currentTime > new TimeSpan(18, 0, 0))
    //            throw new InvalidOperationException("Voting is only allowed between 8 AM and 6 PM.");

    //        if (await _context.Votes.AnyAsync(v => v.VoterId == voter.VoterId))
    //            throw new InvalidOperationException("You have already voted.");

    //        var vote = new Vote
    //        {
    //            VoterId = voter.VoterId,
    //            CandidateId = voteDto.CandidateId,
    //            StateId = voter.StateId,
    //            VoteTime = DateTime.UtcNow
    //        };

    //        _context.Votes.Add(vote);

    //        if (voteDto.CandidateId != null)
    //        {
    //            var candidate = await _context.Candidates.FindAsync(voteDto.CandidateId);
    //            if (candidate == null)
    //                throw new KeyNotFoundException("Candidate not found.");

    //            if (candidate.StateId != voter.StateId)
    //                throw new UnauthorizedAccessException("You can only vote for candidates in your own state.");

    //            candidate.VoteCount += 1;
    //        }

    //        await _context.SaveChangesAsync();
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception($"Error while casting vote: {ex.Message}");
    //    }
    //}

    public async Task<List<CandidateResponseDTO>> GetCandidatesByVoterStateAsync(string voterCardNumber)
    {
        try
        {
            var voter = await _context.Voters
                .FirstOrDefaultAsync(v => v.VoterCardNumber == voterCardNumber);

            if (voter == null)
                throw new KeyNotFoundException("Voter not found.");

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
            throw new Exception($"Error fetching candidates: {ex.Message}");
        }
    }

    public async Task<int> GetTotalVotesAsync()
    {
        try
        {
            return await _context.Votes.CountAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error counting total votes: {ex.Message}");
        }
    }

    public async Task<int> GetVotesByStateAsync(int stateId)
    {
        try
        {
            return await _context.Votes.CountAsync(v => v.StateId == stateId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error counting votes for state {stateId}: {ex.Message}");
        }
    }
}
