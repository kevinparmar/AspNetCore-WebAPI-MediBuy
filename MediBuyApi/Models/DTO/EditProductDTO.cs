using System.ComponentModel.DataAnnotations;

namespace MediBuyApi.Models.DTO
{
    public class EditProductDTO
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Product name cannot be more than 50 characters")]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Product seller name cannot be more than 50 characters")]
        public string Seller { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Product description cannot be more than 1000 characters")]
        public string Description { get; set; }
        [Required]
        public int Availability { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
