
using Microsoft.EntityFrameworkCore;
using VotingSystem.API.Data;
using VotingSystem.API.DTO.State;
using VotingSystem.API.Models;
using VotingSystem.API.Services.Interface;
namespace VotingSystem.API.Services
{
    public class StateService:IStateService
    {
        private readonly VotingDbContext _context;
        public StateService(VotingDbContext context)
        {
            _context = context;
        }
        public async Task<StateResponseDTO> CreateStateAsync(StateRequestDTO stateDto)
        {
            try
            {
                // Check if state already exists with the same name
                var existingState = await _context.States
                    .FirstOrDefaultAsync(s => s.StateName == stateDto.StateName);
                if (existingState != null)
                {
                    throw new Exception("State with this name already exists.");
                }
                var state = new State
                {
                    StateName = stateDto.StateName
                };
                _context.States.Add(state);
                await _context.SaveChangesAsync();
                return new StateResponseDTO
                {
                    StateId = state.StateId,
                    StateName = state.StateName
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the state: {ex.Message}");
            }
        }
        public async Task<IEnumerable<StateResponseDTO>> GetAllStatesAsync()
        {
            try
            {
                var states = await _context.States.ToListAsync();
                return states.Select(s => new StateResponseDTO
                {
                    StateId = s.StateId,
                    StateName = s.StateName
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the states: {ex.Message}");
            }
        }
        public async Task<StateResponseDTO?> GetStateByIdAsync(int stateId)
        {
            try
            {
                var state = await _context.States
                    .FirstOrDefaultAsync(s => s.StateId == stateId);
                if (state == null)
                {
                    return null;
                }
                return new StateResponseDTO
                {
                    StateId = state.StateId,
                    StateName = state.StateName
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the state: {ex.Message}");
            }
        }
        public async Task<bool> DeleteStateAsync(int stateId)
        {
            try
            {
                var state = await _context.States
                    .FirstOrDefaultAsync(s => s.StateId == stateId);
                if (state == null)
                {
                    throw new Exception("State not found.");
                }
                // Check if any candidate is standing in elections from this state
                var candidatesInState = await _context.Candidates
                    .AnyAsync(c => c.StateId == stateId);
                if (candidatesInState)
                {
                    throw new Exception("Cannot delete state with candidates standing for elections.");
                }
                _context.States.Remove(state);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the state: {ex.Message}");
            }
        }
        public async Task<StateResponseDTO?> UpdateStateAsync(int stateId, StateRequestDTO stateDto)
        {
            try
            {
                var state = await _context.States
                    .FirstOrDefaultAsync(s => s.StateId == stateId);
                if (state == null)
                {
                    throw new Exception("State not found.");
                }
                // Check if another state exists with the same name
                var existingState = await _context.States
                    .FirstOrDefaultAsync(s => s.StateName == stateDto.StateName && s.StateId != stateId);
                if (existingState != null)
                {
                    throw new Exception("State with this name already exists.");
                }
                state.StateName = stateDto.StateName;
                await _context.SaveChangesAsync();
                return new StateResponseDTO
                {
                    StateId = state.StateId,
                    StateName = state.StateName
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the state: {ex.Message}");
            }
        }
    }
}
    
