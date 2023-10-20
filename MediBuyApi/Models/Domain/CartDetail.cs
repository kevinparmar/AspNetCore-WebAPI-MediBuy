using System.ComponentModel.DataAnnotations.Schema;

namespace MediBuyApi.Models.Domain
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        public int CartId { get; set; }       
        public int ProductId { get; set; }       
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        //Navigation Properties
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
