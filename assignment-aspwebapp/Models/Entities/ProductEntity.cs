using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment_aspwebapp.Models.Entities
{
    public class ProductEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string ShortDescription { get; set; } = null!;
        [Required]
        public string LongDescription { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        [Required]
        public string? ImageName { get; set; }
        public string? ImageAlt { get; set; }
        [Required]
        public string Category { get; set; } = null!;
        public string? Tag { get; set; }
    }
}
