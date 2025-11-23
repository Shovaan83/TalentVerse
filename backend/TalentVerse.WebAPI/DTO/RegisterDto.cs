using System.ComponentModel.DataAnnotations;

namespace TalentVerse.WebAPI.DTO.Account
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        public string Username { get; set; } 

        [Required]
        public string Password { get; set; }

        public string? Bio { get; set; }
    }
}
