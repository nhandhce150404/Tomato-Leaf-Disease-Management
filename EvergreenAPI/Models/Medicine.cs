using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Medicine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicineId { get; set; }
        [Required]
        [DisplayName("Medicine Name")]
        public string Name { get; set; }
        [Required]
        public string Uses { get; set; }
        [DisplayName("Medicine Category")]
        public int MedicineCategoryId { get; set; }
        [ForeignKey("MedicineCategoryId")]
        public virtual MedicineCategory MedicineCategory { get; set; }
        [DisplayName("Description")]
        public int? ThumbnailId { get; set; }
        [ForeignKey("ThumbnailId")]
        public virtual Thumbnail Thumbnail { get; set; }
    }
}
