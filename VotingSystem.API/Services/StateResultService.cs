using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Data;
using VotingSystem.API.DTO.StateResult;
using VotingSystem.API.Models;
using VotingSystem.API.Services.Interface;

namespace VotingSystem.API.Services
{
    public class StateResultService : IStateResultService
    {
        private readonly VotingDbContext _context;
        private readonly ILogger<StateResultService> _logger; 

        public StateResultService(VotingDbContext context, ILogger<StateResultService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Method to declare the winner for a state
        public async Task<StateResultResponseDTO> DeclareWinnerAsync(int stateId)
        {
            try
            {
                _logger.LogInformation($"Starting DeclareWinnerAsync for StateId: {stateId}");

                // ✅ Check if the state exists
                var state = await _context.States.FirstOrDefaultAsync(s => s.StateId == stateId);
                if (state == null)
                {
                    _logger.LogError($"State not found for StateId: {stateId}");
                    throw new Exception("State not found.");
                }

                // ✅ Get votes for candidates in the state
                var candidateVotes = await _context.Votes
                    .Where(v => v.StateId == stateId && v.CandidateId != null)
                    .GroupBy(v => v.CandidateId)
                    .Select(group => new
                    {
                        CandidateId = group.Key,
                        VoteCount = group.Count()
                    })
                    .OrderByDescending(v => v.VoteCount)
                    .ToListAsync();

                // ✅ Check if all votes are abstentions
                if (!candidateVotes.Any())
                {
                    _logger.LogWarning($"All votes in StateId: {stateId} are abstentions. No winner.");
                    return new StateResultResponseDTO
                    {
                        StateId = stateId,
                        StateName = state.StateName,
                        WinningCandidateName = "No winner. All votes were abstentions. A re-election is required.",
                        TotalVotes = 0
                    };
                }

                //  Find the highest vote count
                var highestVoteCount = candidateVotes.First().VoteCount;
                var topCandidates = candidateVotes.Where(v => v.VoteCount == highestVoteCount).ToList();

                //  Handle a tie (election draw)
                if (topCandidates.Count > 1)
                {
                    _logger.LogWarning($"Election draw detected for StateId: {stateId}");
                    return new StateResultResponseDTO
                    {
                        StateId = stateId,
                        StateName = state.StateName,
                        WinningCandidateName = "Election results are not declared due to a draw. Another round of voting will happen.",
                        TotalVotes = candidateVotes.Sum(v => v.VoteCount)
                    };
                }

                //  Declare the winner
                var winningCandidateId = topCandidates.First().CandidateId;

                var winningCandidate = await _context.Candidates
                    .FirstOrDefaultAsync(c => c.CandidateId == winningCandidateId);
                if (winningCandidate == null)
                {
                    _logger.LogError($"Winning candidate not found for CandidateId: {winningCandidateId}");
                    throw new Exception("Winning candidate not found.");
                }

                //  Save the result in the StateResult table
                var stateResult = new StateResult
                {
                    StateId = stateId,
                    WinningCandidateId = winningCandidateId.Value,
                    TotalVotes = candidateVotes.Sum(v => v.VoteCount)
                };

                _context.StateResults.Add(stateResult);
                await _context.SaveChangesAsync();

                //  Fetch the saved result to ensure StateResultId is correctly assigned
                var savedStateResult = await _context.StateResults
                    .FirstOrDefaultAsync(sr => sr.StateId == stateId && sr.WinningCandidateId == winningCandidateId);

                _logger.LogInformation($"Winner declared for StateId: {stateId}, Candidate: {winningCandidate.CandidateName}");

                return new StateResultResponseDTO
                {
                    StateResultId = savedStateResult?.StateResultId ?? 0, // Ensure a valid ID is returned
                    StateId = stateId,
                    StateName = state.StateName,
                    WinningCandidateId = winningCandidate.CandidateId,
                    WinningCandidateName = winningCandidate.CandidateName,
                    TotalVotes = stateResult.TotalVotes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while declaring winner for StateId: {stateId}. Exception: {ex.Message}");
                throw new Exception("Winner has already been declared.", ex);
            }
        }

        //public async Task<StateResultResponseDTO> DeclareWinnerAsync(int stateId)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Starting DeclareWinnerAsync for StateId: {stateId}");

        //        // Check if the state exists
        //        var state = await _context.States.FirstOrDefaultAsync(s => s.StateId == stateId);
        //        if (state == null)
        //        {
        //            _logger.LogError($"State not found for StateId: {stateId}");
        //            throw new Exception("State not found.");
        //        }


        //        var candidateVotes = await _context.Votes
        //            .Where(v => v.StateId == stateId && v.CandidateId != null) 
        //            .GroupBy(v => v.CandidateId)
        //            .Select(group => new
        //            {
        //                CandidateId = group.Key,
        //                VoteCount = group.Count()
        //            })
        //            .OrderByDescending(v => v.VoteCount)
        //            .ToListAsync();


        //        //if (!candidateVotes.Any())
        //        //{
        //        //    _logger.LogError($"No valid votes found for StateId: {stateId}");
        //        //    throw new Exception("No votes have been cast for this state.");
        //        //}
        //        if (!candidateVotes.Any())
        //        {
        //            _logger.LogWarning($"All votes in StateId: {stateId} are abstentions. No winner.");
        //            return new StateResultResponseDTO
        //            {
        //                StateId = stateId,
        //                StateName = state.StateName,
        //                WinningCandidateName = "No winner. All votes were abstentions. A re-election is required.",
        //                TotalVotes = 0,
        //            };
        //        }


        //        var highestVoteCount = candidateVotes.First().VoteCount;
        //        var topCandidates = candidateVotes.Where(v => v.VoteCount == highestVoteCount).ToList();

        //        if (topCandidates.Count > 1)
        //        {
        //            _logger.LogWarning($"Election draw detected for StateId: {stateId}");
        //            return new StateResultResponseDTO
        //            {
        //                StateId = stateId,
        //                StateName = state.StateName,
        //                WinningCandidateName = "Election results are not declared due to a draw. Another round of voting will happen.",
        //                TotalVotes = candidateVotes.Sum(v => v.VoteCount)
        //            };
        //        }

        //        // ✅ Declare the winner
        //        var winningCandidateId = topCandidates.First().CandidateId;

        //        var winningCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.CandidateId == winningCandidateId);
        //        if (winningCandidate == null)
        //        {
        //            _logger.LogError($"Winning candidate not found for CandidateId: {winningCandidateId}");
        //            throw new Exception("Winning candidate not found.");
        //        }

        //        // ✅ Save the result in StateResult table
        //        var stateResult = new StateResult
        //        {
        //            StateId = stateId,
        //            WinningCandidateId = winningCandidateId.Value,
        //            TotalVotes = candidateVotes.Sum(v => v.VoteCount)
        //        };

        //        _context.StateResults.Add(stateResult);
        //        await _context.SaveChangesAsync();

        //        _logger.LogInformation($"Winner declared for StateId: {stateId}, Candidate: {winningCandidate.CandidateName}");

        //        return new StateResultResponseDTO
        //        {
        //            StateId = stateId,
        //            StateName = state.StateName,
        //            WinningCandidateName = winningCandidate.CandidateName,
        //            WinningCandidateId=winningCandidate.CandidateId,
        //            TotalVotes = stateResult.TotalVotes
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error while declaring winner for StateId: {stateId}. Exception: {ex.Message}");
        //        throw new Exception("Winner has already been declared.", ex);
        //    }
        //}


        public async Task<StateResultResponseDTO> GetStateResultByStateIdAsync(int stateId)
        {
            try
            {
                _logger.LogInformation($"Fetching state result for StateId: {stateId}");

                var stateResult = await _context.StateResults
                    .Include(sr => sr.WinningCandidate) 
                    .Include(sr => sr.State) 
                    .FirstOrDefaultAsync(sr => sr.StateId == stateId);

                if (stateResult == null)
                {
                    _logger.LogError($"No state result found for StateId: {stateId}");
                    throw new Exception($"No state result found for StateId: {stateId}. Please ensure the state has been populated with election results.");
                }

                _logger.LogInformation($"State result retrieved successfully for StateId: {stateId}");

                // ✅ Return only necessary fields in DTO format to avoid cycles
                return new StateResultResponseDTO
                {
                    StateId = stateResult.StateId,
                    StateName = stateResult.State.StateName,
                    WinningCandidateName = stateResult.WinningCandidate.CandidateName,
                    TotalVotes = stateResult.TotalVotes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving state result for StateId: {stateId}. Exception: {ex.Message}");
                throw new Exception($"Error while retrieving state result for StateId: {stateId}. Error Details: {ex.Message}", ex);
            }
        }

    }
}
