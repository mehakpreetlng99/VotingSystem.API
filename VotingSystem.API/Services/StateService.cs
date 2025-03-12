
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Data;
using VotingSystem.API.DTO.State;
using VotingSystem.API.Models;
using VotingSystem.API.Services.Interface;

namespace VotingSystem.API.Services
{
    public class StateService : IStateService
    {
        private readonly VotingDbContext _context;
        private readonly ILogger<StateService> _logger;  

        public StateService(VotingDbContext context, ILogger<StateService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<StateResponseDTO> CreateStateAsync(StateRequestDTO stateDto)
        {
            try
            {
                _logger.LogInformation("Creating state: {StateName}", stateDto.StateName);

                var existingState = await _context.States
                    .FirstOrDefaultAsync(s => s.StateName == stateDto.StateName);

                if (existingState != null)
                {
                    _logger.LogWarning("State with name {StateName} already exists", stateDto.StateName);
                    throw new InvalidOperationException("State with this name already exists.");
                }

                var state = new State { StateName = stateDto.StateName };
                _context.States.Add(state);
                await _context.SaveChangesAsync();

                _logger.LogInformation("State created successfully with ID {StateId}", state.StateId);

                return new StateResponseDTO { StateId = state.StateId, StateName = state.StateName };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating state {StateName}", stateDto.StateName);
                throw;
            }
        }

        public async Task<IEnumerable<StateResponseDTO>> GetAllStatesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all states.");
                var states = await _context.States.AsNoTracking().ToListAsync();

                return states.Select(s => new StateResponseDTO
                {
                    StateId = s.StateId,
                    StateName = s.StateName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving states.");
                throw;
            }
        }

        public async Task<StateResponseDTO?> GetStateByIdAsync(int stateId)
        {
            try
            {
                _logger.LogInformation("Fetching state with ID {StateId}", stateId);

                var state = await _context.States
                    .AsNoTracking()
                    .SingleOrDefaultAsync(s => s.StateId == stateId);

                if (state == null)
                {
                    _logger.LogWarning("State with ID {StateId} not found", stateId);
                    return null;
                }

                return new StateResponseDTO { StateId = state.StateId, StateName = state.StateName };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving state with ID {StateId}", stateId);
                throw;
            }
        }

        public async Task<bool> DeleteStateAsync(int stateId)
        {
            try
            {
                _logger.LogInformation("Deleting state with ID {StateId}", stateId);

                var state = await _context.States.SingleOrDefaultAsync(s => s.StateId == stateId);
                if (state == null)
                {
                    _logger.LogWarning("State with ID {StateId} not found", stateId);
                    throw new KeyNotFoundException("State not found.");
                }

                if (await _context.Candidates.AnyAsync(c => c.StateId == stateId))
                {
                    _logger.LogWarning("Cannot delete state {StateId} as candidates exist", stateId);
                    throw new InvalidOperationException("Cannot delete state with candidates.");
                }

                _context.States.Remove(state);
                await _context.SaveChangesAsync();

                _logger.LogInformation("State with ID {StateId} deleted successfully", stateId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting state {StateId}", stateId);
                throw;
            }
        }

        public async Task<StateResponseDTO?> UpdateStateAsync(int stateId, StateRequestDTO stateDto)
        {
            try
            {
                _logger.LogInformation("Updating state {StateId} with new name {StateName}", stateId, stateDto.StateName);

                var state = await _context.States.SingleOrDefaultAsync(s => s.StateId == stateId);
                if (state == null)
                {
                    _logger.LogWarning("State with ID {StateId} not found", stateId);
                    throw new KeyNotFoundException("State not found.");
                }

                var existingState = await _context.States
                    .AnyAsync(s => s.StateName == stateDto.StateName && s.StateId != stateId);

                if (existingState)
                {
                    _logger.LogWarning("Duplicate state name conflict for {StateName}", stateDto.StateName);
                    throw new InvalidOperationException("State with this name already exists.");
                }

                state.StateName = stateDto.StateName;
                await _context.SaveChangesAsync();

                _logger.LogInformation("State {StateId} updated successfully", stateId);

                return new StateResponseDTO { StateId = state.StateId, StateName = state.StateName };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating state {StateId}", stateId);
                throw;
            }
        }
    }
}

