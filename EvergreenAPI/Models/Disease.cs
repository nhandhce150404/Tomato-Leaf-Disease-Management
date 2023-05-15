using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Disease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiseaseId { get; set; }
        [Required]
        [DisplayName("Disease Name")]
        public string Name { get; set; }
        
        public string Identification { get; set; }
       
        public string Affect { get; set; }


        
        [DisplayName("Disease Category")]
        public int DiseaseCategoryId { get; set; }
        [ForeignKey("DiseaseCategoryId")]
        public virtual DiseaseCategory DiseaseCategory { get; set; }
        
        
        [DisplayName("Description")]
        public int ThumbnailId { get; set; }
        [ForeignKey("ThumbnailId")]
        public virtual Thumbnail Thumbnail { get; set; }


        
        [DisplayName("Medicine")]
        public int MedicineId { get; set; }
        [ForeignKey("MedicineId")]
        public virtual Medicine Medicine { get; set; }

        
        [DisplayName("Treatment")]
        public int TreatmentId { get; set; }
        [ForeignKey("TreatmentId")]
        public virtual Treatment Treatment { get; set; }
        
    }
}
