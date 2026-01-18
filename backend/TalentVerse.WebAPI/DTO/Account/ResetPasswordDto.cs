using System.ComponentModel.DataAnnotations;

namespace TalentVerse.WebAPI.DTO.Account
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
