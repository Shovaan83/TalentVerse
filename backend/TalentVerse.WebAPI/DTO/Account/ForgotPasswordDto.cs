using System.ComponentModel.DataAnnotations;

namespace TalentVerse.WebAPI.DTO.Account
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
