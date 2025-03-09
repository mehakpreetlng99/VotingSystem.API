﻿using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
