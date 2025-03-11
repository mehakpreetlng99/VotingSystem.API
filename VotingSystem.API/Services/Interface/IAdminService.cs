////using Microsoft.EntityFrameworkCore.Storage;
////using VotingSystem.API.Models;

////public interface IAdminService
////{
////    Task<bool> AddCandidateAsync(CandidateDto candidateDto);
////    Task<bool> UpdateCandidateAsync(int candidateId, CandidateDto candidateDto);
////    Task<bool> DeleteCandidateAsync(int candidateId);
////    Task<List<Candidate>> GetAllCandidatesAsync();
////    Task<List<Voter>> GetAllVotersAsync();
////    Task<bool> UpdateElectionResultAsync(ElectionResultDto resultDto);
////    Task<ElectionResult?> GetElectionResultAsync();
////}
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using VotingSystem.API.DTO;
//using VotingSystem.API.Models;
//using VotingSystem.API.Data;
//using Microsoft.EntityFrameworkCore;

//public interface IAdminService
//{
//    // Candidate Management
//    Task<bool> AddCandidateAsync(CandidateDTO candidateDto);
//    Task<List<CandidateDTO>> GetAllCandidatesAsync();

//    // Voter Management
//    Task<List<Voter>> GetAllVotersAsync();

//    // Election Result Management
//    Task<bool> UpdateStateResultAsync(StateResultDTO stateResultDto);
//    Task<bool> UpdateNationalResultAsync(NationalResultDTO nationalResultDto);
//    Task<List<StateResult>> GetStateResultsAsync();
//    Task<NationalResult> GetNationalResultAsync();
//}