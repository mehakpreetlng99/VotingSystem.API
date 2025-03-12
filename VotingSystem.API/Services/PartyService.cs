
using VotingSystem.API.Data;
using VotingSystem.API.DTO.Party;
using VotingSystem.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add this

namespace VotingSystem.API.Services
{
    public class PartyService : IPartyService
    {
        private readonly VotingDbContext _context;
        private readonly ILogger<PartyService> _logger; // Inject Logger

        public PartyService(VotingDbContext context, ILogger<PartyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Create a new party
        public async Task<PartyResponseDTO> CreatePartyAsync(PartyRequestDTO partyDto)
        {
            try
            {
                _logger.LogInformation("Creating a new party: {PartyName}", partyDto.PartyName);

                var existingParty = await _context.Parties
                    .FirstOrDefaultAsync(p => p.PartyName == partyDto.PartyName || p.PartySymbol == partyDto.Symbol);
                if (existingParty != null)
                {
                    _logger.LogWarning("Party creation failed: Party name or symbol already exists.");
                    throw new Exception("Party name or symbol already exists.");
                }

                var party = new Party
                {
                    PartyName = partyDto.PartyName,
                    PartySymbol = partyDto.Symbol
                };

                _context.Parties.Add(party);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Party created successfully with ID {PartyId}", party.PartyId);

                return new PartyResponseDTO
                {
                    PartyId = party.PartyId,
                    PartyName = party.PartyName,
                    Symbol = party.PartySymbol
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating party: {ErrorMessage}", ex.Message);
                throw new Exception($"An error occurred while creating the party: {ex.Message}");
            }
        }

        // Get all parties
        public async Task<IEnumerable<PartyResponseDTO>> GetAllPartiesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all parties");

                var parties = await _context.Parties.ToListAsync();
                return parties.Select(p => new PartyResponseDTO
                {
                    PartyId = p.PartyId,
                    PartyName = p.PartyName,
                    Symbol = p.PartySymbol
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving parties: {ErrorMessage}", ex.Message);
                throw new Exception($"An error occurred while retrieving the parties: {ex.Message}");
            }
        }

        // Get a party by its ID
        public async Task<PartyResponseDTO?> GetPartyByIdAsync(int partyId)
        {
            try
            {
                _logger.LogInformation("Fetching party with ID {PartyId}", partyId);

                var party = await _context.Parties
                    .FirstOrDefaultAsync(p => p.PartyId == partyId);
                if (party == null)
                {
                    _logger.LogWarning("Party with ID {PartyId} not found", partyId);
                    return null;
                }

                return new PartyResponseDTO
                {
                    PartyId = party.PartyId,
                    PartyName = party.PartyName,
                    Symbol = party.PartySymbol
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving party: {ErrorMessage}", ex.Message);
                throw new Exception($"An error occurred while retrieving the party: {ex.Message}");
            }
        }

        // Delete a party
        public async Task<bool> DeletePartyAsync(int partyId)
        {
            try
            {
                _logger.LogInformation("Deleting party with ID {PartyId}", partyId);

                var party = await _context.Parties
                    .FirstOrDefaultAsync(p => p.PartyId == partyId);
                if (party == null)
                {
                    _logger.LogWarning("Party with ID {PartyId} not found", partyId);
                    throw new Exception("Party not found.");
                }

                _context.Parties.Remove(party);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Party with ID {PartyId} deleted successfully", partyId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting party: {ErrorMessage}", ex.Message);
                throw new Exception($"An error occurred while deleting the party: {ex.Message}");
            }
        }

        // Update party details
        public async Task<PartyResponseDTO?> UpdatePartyAsync(int partyId, PartyRequestDTO partyDto)
        {
            try
            {
                _logger.LogInformation("Updating party with ID {PartyId}", partyId);

                var party = await _context.Parties
                    .FirstOrDefaultAsync(p => p.PartyId == partyId);
                if (party == null)
                {
                    _logger.LogWarning("Party with ID {PartyId} not found", partyId);
                    throw new Exception("Party not found.");
                }

                var existingParty = await _context.Parties
                    .FirstOrDefaultAsync(p => (p.PartyName == partyDto.PartyName || p.PartySymbol == partyDto.Symbol) && p.PartyId != partyId);
                if (existingParty != null)
                {
                    _logger.LogWarning("Party update failed: Party name or symbol already exists.");
                    throw new Exception("Party name or symbol already exists.");
                }

                party.PartyName = partyDto.PartyName;
                party.PartySymbol = partyDto.Symbol;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Party with ID {PartyId} updated successfully", partyId);

                return new PartyResponseDTO
                {
                    PartyId = party.PartyId,
                    PartyName = party.PartyName,
                    Symbol = party.PartySymbol
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating party: {ErrorMessage}", ex.Message);
                throw new Exception($"An error occurred while updating the party: {ex.Message}");
            }
        }
    }
}

//using VotingSystem.API.Data;
//using VotingSystem.API.DTO.Party;
//using VotingSystem.API.Models;
//using Microsoft.EntityFrameworkCore;

//namespace VotingSystem.API.Services
//{
//    public class PartyService : IPartyService
//    {
//        private readonly VotingDbContext _context;
//        public PartyService(VotingDbContext context)
//        {
//            _context = context;
//        }
//        // Create a new party
//        public async Task<PartyResponseDTO> CreatePartyAsync(PartyRequestDTO partyDto)
//        {
//            try
//            {
//                // Check if the party name or symbol already exists
//                var existingParty = await _context.Parties
//                    .FirstOrDefaultAsync(p => p.PartyName == partyDto.PartyName || p.PartySymbol == partyDto.Symbol);
//                if (existingParty != null)
//                {
//                    throw new Exception("Party name or symbol already exists.");
//                }
//                // Create a new Party entity
//                var party = new Party
//                {
//                    PartyName = partyDto.PartyName,
//                    PartySymbol = partyDto.Symbol
//                };
//                // Add party to the database
//                _context.Parties.Add(party);
//                await _context.SaveChangesAsync();
//                // Return the created party as a response DTO
//                return new PartyResponseDTO
//                {
//                    PartyId = party.PartyId,
//                    PartyName = party.PartyName,
//                    Symbol = party.PartySymbol
//                };
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while creating the party: {ex.Message}");
//            }
//        }
//        // Get all parties
//        public async Task<IEnumerable<PartyResponseDTO>> GetAllPartiesAsync()
//        {
//            try
//            {
//                var parties = await _context.Parties.ToListAsync();
//                return parties.Select(p => new PartyResponseDTO
//                {
//                    PartyId = p.PartyId,
//                    PartyName = p.PartyName,
//                    Symbol = p.PartySymbol
//                });
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while retrieving the parties: {ex.Message}");
//            }
//        }
//        // Get a party by its ID
//        public async Task<PartyResponseDTO?> GetPartyByIdAsync(int partyId)
//        {
//            try
//            {
//                var party = await _context.Parties
//                    .FirstOrDefaultAsync(p => p.PartyId == partyId);
//                if (party == null)
//                {
//                    return null;
//                }
//                return new PartyResponseDTO
//                {
//                    PartyId = party.PartyId,
//                    PartyName = party.PartyName,
//                    Symbol = party.PartySymbol
//                };
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while retrieving the party: {ex.Message}");
//            }
//        }
//        // Delete a party
//        public async Task<bool> DeletePartyAsync(int partyId)
//        {
//            try
//            {
//                var party = await _context.Parties
//                    .FirstOrDefaultAsync(p => p.PartyId == partyId);
//                if (party == null)
//                {
//                    throw new Exception("Party not found.");
//                }
//                _context.Parties.Remove(party);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while deleting the party: {ex.Message}");
//            }
//        }
//        // Update party details
//        public async Task<PartyResponseDTO?> UpdatePartyAsync(int partyId, PartyRequestDTO partyDto)
//        {
//            try
//            {
//                var party = await _context.Parties
//                    .FirstOrDefaultAsync(p => p.PartyId == partyId);
//                if (party == null)
//                {
//                    throw new Exception("Party not found.");
//                }
//                // Check if the party name or symbol already exists
//                var existingParty = await _context.Parties
//                    .FirstOrDefaultAsync(p => (p.PartyName == partyDto.PartyName || p.PartySymbol == partyDto.Symbol) && p.PartyId != partyId);
//                if (existingParty != null)
//                {
//                    throw new Exception("Party name or symbol already exists.");
//                }
//                party.PartyName = partyDto.PartyName;
//                party.PartySymbol = partyDto.Symbol;
//                await _context.SaveChangesAsync();
//                return new PartyResponseDTO
//                {
//                    PartyId = party.PartyId,
//                    PartyName = party.PartyName,
//                    Symbol = party.PartySymbol
//                };
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while updating the party: {ex.Message}");
//            }
//        }
//    }
//}