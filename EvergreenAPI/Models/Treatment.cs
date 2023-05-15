using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Treatment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TreatmentId { get; set; }
        [Required]
        [DisplayName("Treatment Name")]
        public string TreatmentName { get; set; }
        
        public string Method { get; set; }
     
        
        [DisplayName("Description")]
        public int? ThumbnailId { get; set; }
        [ForeignKey("ThumbnailId")]
        public virtual Thumbnail Thumbnail { get; set; }



    }
}
