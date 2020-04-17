using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CaseStudy.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; } // generates FK
        [Required]
        public int BrandId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string GraphicName { get; set; }
        [Required]
        public double CostPrice { get; set; }
        [Required]
        public double MSRP { get; set; }
        [Required]
        public int QtyOnHand { get; set; }
        [Required]
        [StringLength(200)]
        public int QtyOnBackOrder { get; set; }
        [Required]
        public string Description { get; set; }
    }
}