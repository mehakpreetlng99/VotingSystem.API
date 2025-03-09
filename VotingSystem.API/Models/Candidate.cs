using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VotingSystem.API.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }

        [Required, StringLength(100)]
        public string CandidateName { get; set; } = string.Empty;

        [ForeignKey("Party")]
        public int PartyId { get; set; }

        public int StateId { get; set; }
        public bool IsActive { get; set; } = true;

        //Navigation Properties
        public Party? Party { get; set; }
        public State? State { get; set; }
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
