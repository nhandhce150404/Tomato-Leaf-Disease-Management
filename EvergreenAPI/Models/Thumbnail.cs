using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Thumbnail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ThumbnailId { get; set; }
        [Required]
        [DisplayName("Description")]
        public string AltText { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
