//using System.Collections.Generic;
//using System.Threading.Tasks;
//using VotingSystem.API.DTO.Candidate;
//using VotingSystem.API.Models;
//using VotingSystem.API.Data;
//using Microsoft.EntityFrameworkCore;

//public class AdminService : IAdminService
//{
//    private readonly VotingDbContext _context;

//    public AdminService(VotingDbContext context)
//    {
//        _context = context;
//    }

//    // Candidate Management
//    public async Task<bool> AddCandidateAsync(CandidateDTO candidateDto)
//    {
//        var candidate = new Candidate
//        {
//            CandidateName = candidateDto.Name,
//            PartyId = candidateDto.PartyId,
//            StateId = candidateDto.StateId,
//        };
//        _context.Candidates.Add(candidate);
//        await _context.SaveChangesAsync();
//        return true;
//    }

//    //public async Task<List<Candidate>> GetAllCandidatesAsync()
//    //{
//    //    return await _context.Candidates
//    //        .Include(c => c.Party)
//    //        .Include(c => c.State)
//    //        .ToListAsync();
//    //}
//    public async Task<List<CandidateDTO>> GetAllCandidatesAsync()
//    {
//        return await _context.Candidates
//            .Include(c => c.Party)
//            .Include(c => c.State)
//            .Select(c => new CandidateDTO
//            {
//             Name=c.CandidateName,
//             PartyId=c.PartyId,
//             StateId=c.StateId
//            })
//            .ToListAsync();
//    }

//    // Voter Management
//    public async Task<List<Voter>> GetAllVotersAsync()
//    {
//        return await _context.Voters.ToListAsync();
//    }

//    // Election Result Management
//    public async Task<bool> UpdateStateResultAsync(StateResultDTO stateResultDto)
//    {
//        var result = await _context.StateResults.FirstOrDefaultAsync(r => r.StateId == stateResultDto.StateId);
//        if (result != null)
//        {
//            result.WinningCandidateId = stateResultDto.WinningCandidateId; // Use WinningCandidateId instead
//            result.TotalVotes = stateResultDto.TotalVotes;
//        }
//        else
//        {
//            _context.StateResults.Add(new StateResult
//            {
//                StateId = stateResultDto.StateId,
//                WinningCandidateId = stateResultDto.WinningCandidateId, // Use WinningCandidateId instead
//                TotalVotes = stateResultDto.TotalVotes
//            });
//        }
//        await _context.SaveChangesAsync();
//        return true;
//    }


//    public async Task<bool> UpdateNationalResultAsync(NationalResultDTO nationalResultDto)
//    {
//        var result = await _context.NationalResults.FirstOrDefaultAsync();
//        if (result != null)
//        {
//            result.PartyId = nationalResultDto.PartyId;  // ✅ Corrected
//            result.TotalVotes = nationalResultDto.TotalVotes;
//        }
//        else
//        {
//            _context.NationalResults.Add(new NationalResult
//            {
//                PartyId = nationalResultDto.PartyId,  // ✅ Corrected
//                TotalVotes = nationalResultDto.TotalVotes
//            });
//        }
//        await _context.SaveChangesAsync();
//        return true;
//    }



//    public async Task<List<StateResult>> GetStateResultsAsync()
//    {
//        return await _context.StateResults
//            .Include(r => r.State)
//            .Include(r => r.WinningCandidate)  
//            .ToListAsync();
//    }


//    //public async Task<NationalResult?> GetNationalResultAsync()
//    //{
//    //    return await _context.NationalResults
//    //        .Include(r => r.Party)
//    //        .FirstOrDefaultAsync();
//    //}
//    public async Task<NationalResult> GetNationalResultAsync()
//    {
//        return await _context.NationalResults
//            .Include(r => r.Party)
//            .FirstOrDefaultAsync() ?? new NationalResult { PartyId = 0, TotalVotes = 0 };
//    }
//}
