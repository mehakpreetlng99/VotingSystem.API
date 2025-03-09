using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.Models
{
    public class NationalResult
    {
        [Key]
        public int NationalResultId { get; set; }

        [ForeignKey("Party")]
        public int WinningPartyId { get; set; }

        [Required]
        public int TotalVotes { get; set; }


        //Navigation Property
        public Party? WinningParty { get; set; }
    }
}
