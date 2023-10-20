using System.ComponentModel.DataAnnotations.Schema;

namespace MediBuyApi.Models.Domain
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Seller { get; set; }
        public string Description { get; set; }
        public int Availability { get; set; }
        public int CategoryId { get; set; }

        //Navigation Properties

        public Category Category { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
