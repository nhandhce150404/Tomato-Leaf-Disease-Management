using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class ImageDto
    {
        public int ImageId { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string AltText { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(500)]
        public string Url { get; set; }
    }
}
