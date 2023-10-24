namespace MediBuyApi.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }

        //Navigation Properties
        public ICollection<OrderDetailDTO> OrderDetails { get; set; }
    }
}
