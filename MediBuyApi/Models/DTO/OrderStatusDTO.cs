namespace MediBuyApi.Models.DTO
{
    public class OrderStatusDTO
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        //Navigation Properties
        public ICollection<Order> Orders { get; set; }
    }
}
