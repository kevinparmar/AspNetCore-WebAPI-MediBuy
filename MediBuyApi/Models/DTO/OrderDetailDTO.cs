namespace MediBuyApi.Models.DTO
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        //Navigation Properties
        public ProductDTO Product { get; set; }
    }
}
