using Microsoft.EntityFrameworkCore;
using VotingSystem.API.Data;
using VotingSystem.API.DTO.NationalResult;
using VotingSystem.API.Services.Interface;
namespace VotingSystem.API.Services
{
    public class NationalResultService:INationalResultService
    {
        private readonly VotingDbContext _context;
        private readonly ILogger<NationalResultService> _logger;
        public NationalResultService(VotingDbContext context, ILogger<NationalResultService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<NationalResultResponseDTO> GetNationalResultAsync()
        {
            try
            {
                _logger.LogInformation("Calculating national result.");
                
                var stateResults = await _context.StateResults
                    .Include(sr => sr.WinningCandidate)
                    .Include(sr => sr.State)
                    .ToListAsync();
                if (!stateResults.Any())
                {
                    _logger.LogWarning("No state results found.");
                    return new NationalResultResponseDTO
                    {
                        Message = "No results available."
                    };
                }
                var partyStateWins = stateResults
                    .GroupBy(sr => sr.WinningCandidate.PartyId)
                    .Select(group => new
                    {
                        PartyId = group.Key,
                        StateWins = group.Count()
                    })
                    .OrderByDescending(x => x.StateWins)
                    .FirstOrDefault();
                if (partyStateWins == null)
                {
                    _logger.LogWarning("No party wins detected.");
                    return new NationalResultResponseDTO
                    {
                        Message = "No national winner detected."
                    };
                }
                var winningParty = await _context.Parties
                    .FirstOrDefaultAsync(p => p.PartyId == partyStateWins.PartyId);
                if (winningParty == null)
                {
                    _logger.LogError("Winning party not found.");
                    return new NationalResultResponseDTO
                    {
                        Message = "Error finding the winning party."
                    };
                }
                _logger.LogInformation($"National winner is: {winningParty.PartyName}");
                return new NationalResultResponseDTO
                {
                    NationalWinnerPartyName = winningParty.PartyName,
                    StatesWon = partyStateWins.StateWins,
                    TotalStates = stateResults.Select(sr => sr.State).Distinct().Count(),
                    Message = "National results calculated successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calculating national result. Exception: {ex.Message}");
                throw new Exception("An error occurred while calculating the national result.", ex);
            }
        }
    }
}
