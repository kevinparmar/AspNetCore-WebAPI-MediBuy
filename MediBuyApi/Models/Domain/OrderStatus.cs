using System.ComponentModel.DataAnnotations.Schema;

namespace MediBuyApi.Models.Domain
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        //Navigation Properties
        public ICollection<Order> Orders { get; set; }
    }
}
