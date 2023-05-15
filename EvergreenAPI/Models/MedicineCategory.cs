using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class MedicineCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicineCategoryId { get; set; }
        [Required]
        [DisplayName("Medicine Category")]
        public string Name { get; set; }
    }
}
