using System.ComponentModel.DataAnnotations;

namespace MediBuyApi.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public string Seller { get; set; }
        public string Description { get; set; }
        public int Availability { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } 
    }
}
