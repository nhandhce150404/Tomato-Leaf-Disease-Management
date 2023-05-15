using System;
using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class BlogDto
    {
        public int BlogId { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        [Required]
        [StringLength(1000000000)]
        [MinLength(100)]
        public string Content { get; set; }
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        public int ViewCount { get; set; } = 0;
        [Required]
        public int ThumbnailId { get; set; }
    }
}
