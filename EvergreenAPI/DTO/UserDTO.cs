using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.DTO
{
    public class UserDto
    {
        public int AccountId { get; set; }


        [Column(TypeName = "nvarchar(255)")]
        public string Username { get; set; }


        [Required]
        public string Role { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string FullName { get; set; }



        [MaxLength(255)]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email, Please Re-Enter")]
        public string Email { get; set; } = string.Empty;



        [Required(ErrorMessage = "Cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$", ErrorMessage = "Use more than 7 characters include letters and numbers")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;



        [NotMapped]
        [Required(ErrorMessage = "Cannot be blank"), Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;




        public string Professions { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBlocked { get; set; } = false;


    }
}
