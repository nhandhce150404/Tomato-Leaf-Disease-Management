using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class DetectionAccuracy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetectionAccuracyId { get; set; }
        [Required]
        public double Accuracy { get; set; }

        [ForeignKey(nameof(DetectionHistory))]
        public int DetectionHistoryId { get; set; }
        public virtual DetectionHistory DetectionHistory { get; set; }

        [ForeignKey(nameof(Disease))]
        public int DiseaseId { get; set; }
        public virtual Disease Disease { get; set; }
    }
}
