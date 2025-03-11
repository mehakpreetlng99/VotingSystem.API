using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.Models
{
    public class NationalResult
    {
        [Key]
        public int NationalResultId { get; set; }

        [ForeignKey("Party")]
        public int PartyId { get; set; }

        [Required]
        public int TotalVotes { get; set; }

        //Navigation Property
        public Party? Party { get; set; }
    }
}
