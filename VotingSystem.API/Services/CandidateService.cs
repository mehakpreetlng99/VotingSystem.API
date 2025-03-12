
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Data;
using VotingSystem.API.DTO.Candidate;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly VotingDbContext _dbContext;
        private readonly ILogger<CandidateService> _logger;

        public CandidateService(VotingDbContext dbContext, ILogger<CandidateService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<CandidateResponseDTO>> GetAllCandidatesAsync()
        {
            _logger.LogInformation("Fetching all candidates from the database.");
            return await _dbContext.Candidates
                .Include(c => c.State)
                .Include(c => c.Party)
                .Select(c => new CandidateResponseDTO
                {
                    CandidateId = c.CandidateId,
                    CandidateName = c.CandidateName,
                    StateId = c.StateId,
                    StateName = c.State.StateName,
                    PartyId = c.PartyId,
                    PartyName = c.Party.PartyName
                })
                .ToListAsync();
        }

        public async Task<CandidateResponseDTO?> GetCandidateByIdAsync(int id)
        {
            _logger.LogInformation("Fetching candidate with ID {CandidateId}", id);

            var candidate = await _dbContext.Candidates
                .Include(c => c.State)
                .Include(c => c.Party)
                .FirstOrDefaultAsync(c => c.CandidateId == id);

            if (candidate == null)
            {
                _logger.LogWarning("Candidate with ID {CandidateId} not found.", id);
                return null;
            }

            return new CandidateResponseDTO
            {
                CandidateId = candidate.CandidateId,
                CandidateName = candidate.CandidateName,
                StateId = candidate.StateId,
                StateName = candidate.State?.StateName,
                PartyId = candidate.PartyId,
                PartyName = candidate.Party?.PartyName
            };
        }

        public async Task<CandidateResponseDTO?> CreateCandidateAsync(CandidateRequestDTO requestDto)
        {
            if (requestDto == null)
            {
                _logger.LogError("CandidateRequestDTO is null.");
                throw new ArgumentNullException(nameof(requestDto), "CandidateRequestDTO is required.");
            }

            if (string.IsNullOrWhiteSpace(requestDto.CandidateName))
            {
                _logger.LogError("Candidate name cannot be empty.");
                throw new ArgumentException("Candidate name is required.", nameof(requestDto.CandidateName));
            }

            try
            {
                _logger.LogInformation("Attempting to create a new candidate: Name={CandidateName}, PartyId={PartyId}, StateId={StateId}",
                    requestDto.CandidateName, requestDto.PartyId, requestDto.StateId);

                // Check if the provided PartyId exists
                var party = await _dbContext.Parties
                    .Where(p => p.PartyId == requestDto.PartyId)
                    .Select(p => new { p.PartyId, p.PartyName })
                    .FirstOrDefaultAsync();

                if (party == null)
                {
                    _logger.LogWarning("Invalid PartyId={PartyId}. Party does not exist.", requestDto.PartyId);
                    throw new KeyNotFoundException($"The party with ID {requestDto.PartyId} does not exist.");
                }

                // Check if the provided StateId exists
                var state = await _dbContext.States
                    .Where(s => s.StateId == requestDto.StateId)
                    .Select(s => new { s.StateId, s.StateName })
                    .FirstOrDefaultAsync();

                if (state == null)
                {
                    _logger.LogWarning("Invalid StateId={StateId}. State does not exist.", requestDto.StateId);
                    throw new KeyNotFoundException($"The state with ID {requestDto.StateId} does not exist.");
                }

                // Check if a candidate from the same party already exists in the given state
                bool candidateExists = await _dbContext.Candidates
                    .AnyAsync(c => c.PartyId == requestDto.PartyId && c.StateId == requestDto.StateId);

                if (candidateExists)
                {
                    _logger.LogWarning("A candidate from PartyId={PartyId} already exists in StateId={StateId}.",
                        requestDto.PartyId, requestDto.StateId);
                    throw new InvalidOperationException("A candidate from the same party already exists in this state.");
                }

                var candidate = new Candidate
                {
                    CandidateName = requestDto.CandidateName,
                    StateId = requestDto.StateId,
                    PartyId = requestDto.PartyId
                };

                _dbContext.Candidates.Add(candidate);
                await _dbContext.SaveChangesAsync();

                var response = new CandidateResponseDTO
                {
                    CandidateId = candidate.CandidateId,
                    CandidateName = candidate.CandidateName,
                    StateId = candidate.StateId,
                    StateName = state.StateName,  // Using validated state data
                    PartyId = candidate.PartyId,
                    PartyName = party.PartyName  // Using validated party data
                };

                _logger.LogInformation("Candidate successfully created with CandidateId={CandidateId}", candidate.CandidateId);

                return response;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Invalid PartyId or StateId provided.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Candidate creation failed due to business logic violation.");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while saving candidate.");
                throw new Exception("An error occurred while saving the candidate to the database. Please try again.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating candidate.");
                throw new Exception("An unexpected error occurred while creating the candidate.", ex);
            }
        }


        //public async Task<CandidateResponseDTO?> CreateCandidateAsync(CandidateRequestDTO requestDto)
        //{
        //    if (requestDto == null)
        //    {
        //        _logger.LogError("CandidateRequestDTO is null.");
        //        throw new ArgumentNullException(nameof(requestDto), "CandidateRequestDTO is null.");
        //    }

        //    try
        //    {
        //        _logger.LogInformation("Creating a new candidate: Name={CandidateName}, PartyId={PartyId}, StateId={StateId}",
        //            requestDto.CandidateName, requestDto.PartyId, requestDto.StateId);

        //        bool candidateExists = await _dbContext.Candidates
        //            .AnyAsync(c => c.PartyId == requestDto.PartyId && c.StateId == requestDto.StateId);

        //        if (candidateExists)
        //        {
        //            _logger.LogWarning("A candidate from the same party already exists in state ID {StateId}.", requestDto.StateId);
        //            throw new InvalidOperationException("A candidate from the same party already exists in this state.");
        //        }

        //        var candidate = new Candidate
        //        {
        //            CandidateName = requestDto.CandidateName,
        //            StateId = requestDto.StateId,
        //            PartyId = requestDto.PartyId
        //        };

        //        _dbContext.Candidates.Add(candidate);
        //        await _dbContext.SaveChangesAsync();

        //        return new CandidateResponseDTO
        //        {
        //            CandidateId = candidate.CandidateId,
        //            CandidateName = candidate.CandidateName,
        //            StateId = candidate.StateId,
        //            StateName = candidate.State?.StateName,
        //            PartyId = candidate.PartyId,
        //            PartyName = candidate.Party?.PartyName
        //        };
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        _logger.LogError(ex, "Candidate creation failed due to business logic violation.");
        //        throw;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        _logger.LogError(ex, "Database error while saving candidate.");
        //        throw new Exception("An error occurred while saving the candidate to the database. Please try again.", ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Unexpected error while creating candidate.");
        //        throw;
        //    }
        //}

        public async Task<CandidateResponseDTO?> UpdateCandidateAsync(int id, CandidateRequestDTO requestDto)
        {
            _logger.LogInformation("Updating candidate with ID {CandidateId}.", id);

            var candidate = await _dbContext.Candidates.FindAsync(id);
            if (candidate == null)
            {
                _logger.LogWarning("Candidate with ID {CandidateId} not found.", id);
                return null;
            }

            bool candidateExists = await _dbContext.Candidates
                .AnyAsync(c => c.PartyId == requestDto.PartyId && c.StateId == requestDto.StateId && c.CandidateId != id);

            if (candidateExists)
            {
                _logger.LogWarning("A candidate from the same party already exists in state ID {StateId}.", requestDto.StateId);
                throw new InvalidOperationException("A candidate from the same party already exists in this state.");
            }

            candidate.CandidateName = requestDto.CandidateName;
            candidate.StateId = requestDto.StateId;
            candidate.PartyId = requestDto.PartyId;

            await _dbContext.SaveChangesAsync();

            return new CandidateResponseDTO
            {
                CandidateId = candidate.CandidateId,
                CandidateName = candidate.CandidateName,
                StateId = candidate.StateId,
                PartyId = candidate.PartyId,
                PartyName=candidate.Party.PartyName,
                StateName=candidate.State.StateName
            };
        }

        //public async Task<bool> DeleteCandidateAsync(int id)
        //{
        //    _logger.LogInformation("Attempting to delete candidate with ID {CandidateId}.", id);

        //    var candidate = await _dbContext.Candidates.FindAsync(id);
        //    if (candidate == null)
        //    {
        //        _logger.LogWarning("Candidate with ID {CandidateId} not found.", id);
        //        return false;
        //    }

        //    _dbContext.Candidates.Remove(candidate);
        //    await _dbContext.SaveChangesAsync();

        //    _logger.LogInformation("Candidate with ID {CandidateId} deleted successfully.", id);
        //    return true;
        //}
        public async Task<string> DeleteCandidateAsync(int id)
        {
            _logger.LogInformation($"Attempting to delete candidate with ID: {id}");

            var candidate = await _dbContext.Candidates
                                            .Include(c => c.Votes) // Include related votes
                                            .FirstOrDefaultAsync(c => c.CandidateId == id);

            if (candidate == null)
            {
                _logger.LogWarning($"Candidate with ID {id} not found.");
                return "Candidate not found.";
            }

            if (candidate.Votes.Any())
            {
                _logger.LogWarning($"Cannot delete candidate with ID {id} because they have existing votes.");
                return "Cannot delete candidate with existing votes.";
            }

            _dbContext.Candidates.Remove(candidate);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Candidate with ID {id} deleted successfully.");
            return "Candidate deleted successfully.";
        }


    }
}

//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using VotingSystem.API.Data;
//using VotingSystem.API.DTO.Candidate;
//using VotingSystem.API.Models;

//namespace VotingSystem.API.Services
//{
//    public class CandidateService : ICandidateService
//    {
//        private readonly VotingDbContext _dbContext;

//        public CandidateService(VotingDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task<List<CandidateResponseDTO>> GetAllCandidatesAsync()
//        {
//            return await _dbContext.Candidates
//                .Include(c => c.State)
//                .Include(c => c.Party)
//                .Select(c => new CandidateResponseDTO
//                {
//                    CandidateId = c.CandidateId,
//                    CandidateName = c.CandidateName,
//                    StateId = c.StateId,
//                    StateName = c.State.StateName,
//                    PartyId = c.PartyId,
//                    PartyName = c.Party.PartyName,
//                    //IsActive = c.IsActive
//                })
//                .ToListAsync();
//        }

//        public async Task<CandidateResponseDTO?> GetCandidateByIdAsync(int id)
//        {
//            var candidate = await _dbContext.Candidates
//                .Include(c => c.State)
//                .Include(c => c.Party)
//                .FirstOrDefaultAsync(c => c.CandidateId == id);

//            if (candidate == null) return null;

//            return new CandidateResponseDTO
//            {
//                CandidateId = candidate.CandidateId,
//                CandidateName = candidate.CandidateName,
//                StateId = candidate.StateId,
//                StateName = candidate.State?.StateName,
//                PartyId = candidate.PartyId,
//                PartyName = candidate.Party?.PartyName,
//                //IsActive = candidate.IsActive
//            };
//        }

//        public async Task<CandidateResponseDTO?> CreateCandidateAsync(CandidateRequestDTO requestDto)
//        {
//            try
//            {
//                // Check if a candidate from the same party already exists in the given state
//                bool candidateExists = await _dbContext.Candidates
//                    .AnyAsync(c => c.PartyId == requestDto.PartyId && c.StateId == requestDto.StateId);

//                if (candidateExists)
//                {
//                    throw new InvalidOperationException("A candidate from the same party already exists in this state.");
//                }

//                var candidate = new Candidate
//                {
//                    CandidateName = requestDto.CandidateName,
//                    StateId = requestDto.StateId,
//                    PartyId = requestDto.PartyId,
//                    //IsActive = requestDto.IsActive
//                };

//                _dbContext.Candidates.Add(candidate);
//                await _dbContext.SaveChangesAsync();

//                if (requestDto == null)
//                {
//                    throw new ArgumentNullException(nameof(requestDto), "CandidateRequestDTO is null.");
//                }

//                Console.WriteLine($"Candidate Request: Name={requestDto.CandidateName}, PartyId={requestDto.PartyId}, StateId={requestDto.StateId}");

//                var party = await _dbContext.Parties.FindAsync(requestDto.PartyId);
//                if (party == null)
//                {
//                    throw new Exception($"Party with ID {requestDto.PartyId} not found.");
//                }

//                var state = await _dbContext.States.FindAsync(requestDto.StateId);
//                if (state == null)
//                {
//                    throw new Exception($"State with ID {requestDto.StateId} not found.");
//                }

//                return new CandidateResponseDTO
//                {
//                    CandidateId = candidate.CandidateId,
//                    CandidateName = candidate.CandidateName,
//                    StateId = candidate.StateId,
//                    StateName=candidate.State.StateName,
//                    PartyName=candidate.Party.PartyName,
//                    PartyId = candidate.PartyId,
//                    //IsActive = candidate.IsActive
//                };
//            }
//            catch (InvalidOperationException ex)
//            {
//                throw new InvalidOperationException($"Candidate creation failed: {ex.Message}");
//            }
//            catch (DbUpdateException ex)
//            {
//                throw new Exception("An error occurred while saving the candidate to the database. Please try again.", ex);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("An unexpected error occurred while creating the candidate.", ex);
//            }
//        }

//        public async Task<CandidateResponseDTO?> UpdateCandidateAsync(int id, CandidateRequestDTO requestDto)
//        {
//            var candidate = await _dbContext.Candidates.FindAsync(id);
//            if (candidate == null) return null;

//            // Check if updating to a party that already has a candidate in the state
//            bool candidateExists = await _dbContext.Candidates
//                .AnyAsync(c => c.PartyId == requestDto.PartyId && c.StateId == requestDto.StateId && c.CandidateId != id);

//            if (candidateExists)
//            {
//                throw new InvalidOperationException("A candidate from the same party already exists in this state.");
//            }

//            candidate.CandidateName = requestDto.CandidateName;
//            candidate.StateId = requestDto.StateId;
//            candidate.PartyId = requestDto.PartyId;
//            //candidate.IsActive = requestDto.IsActive;

//            await _dbContext.SaveChangesAsync();

//            return new CandidateResponseDTO
//            {
//                CandidateId = candidate.CandidateId,
//                CandidateName = candidate.CandidateName,
//                StateId = candidate.StateId,
//                PartyId = candidate.PartyId,
//                //IsActive = candidate.IsActive
//            };
//        }

//        public async Task<bool> DeleteCandidateAsync(int id)
//        {
//            var candidate = await _dbContext.Candidates.FindAsync(id);
//            if (candidate == null) return false;

//            _dbContext.Candidates.Remove(candidate);
//            await _dbContext.SaveChangesAsync();
//            return true;
//        }
//    }
//}
