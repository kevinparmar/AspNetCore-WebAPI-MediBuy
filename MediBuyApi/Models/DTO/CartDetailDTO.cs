namespace MediBuyApi.Models.DTO
{
    public class CartDetailDTO
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        //Navigation Properties
        public ProductDTO Product { get; set; }
    }
}
