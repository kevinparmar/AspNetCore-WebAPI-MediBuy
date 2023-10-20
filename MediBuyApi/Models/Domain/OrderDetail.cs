using System.ComponentModel.DataAnnotations.Schema;

namespace MediBuyApi.Models.Domain
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }      
        public int ProductId { get; set; }        
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        //Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
