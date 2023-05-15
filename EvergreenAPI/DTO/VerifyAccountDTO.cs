using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class VerifyAccountDto
    {
        [MaxLength(255)]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email, Please Re-Enter")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; }
    }
}
