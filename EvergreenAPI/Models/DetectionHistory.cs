using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public sealed class DetectionHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetectionHistoryId { get; set; }

        [Required] public string ImageName { get; init; } = string.Empty;
        [Required] public string ImageUrl { get; init; } = string.Empty;
        [Required] public DateTime Date { get; set; }

        public bool IsExpertConfirmed { get; set; } = false;

        public Disease DetectedDisease { get; set; }

        [Required]
        [ForeignKey(nameof(Account))]
        public int AccountId { get; init; }

        public Account Account { get; set; }
    }
}