using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.Models
{
    public class Party
    {
        [Key]
        public int PartyId { get; set; }

        [Required, StringLength(100)]
        public string PartyName { get; set; } = string.Empty;

        [Required]
        public string PartySymbol { get; set; } = string.Empty; 

        //Navigation Property
        public List<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
}
