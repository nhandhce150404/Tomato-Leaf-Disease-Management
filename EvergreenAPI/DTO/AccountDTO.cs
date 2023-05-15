using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EvergreenAPI.DTO
{
    public class AccountDto
    {
        public int AccountId { get; set; }


        [Column(TypeName = "nvarchar(255)")] public string Username { get; set; }

        [Column(TypeName = "nvarchar(255)")] public string FullName { get; set; }

        [MaxLength(255)]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Invalid email, Please Re-Enter")]
        public string Email { get; set; } = string.Empty;

        public string Professions { get; set; }
        public DateTime? VerifiedAt { get; set; }


        [Display(Name = "Phone Number")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter correct phone num")]
        [StringLength(11, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        public bool IsBlocked { get; set; } = false;
        public string AvatarUrl { get; set; } = "https://i.imgur.com/n1rrde0.png";

        public IFormFile AvatarImg { get; set; }
        public string Bio { get; set; }
        public string Role { get; set; }
    }
}