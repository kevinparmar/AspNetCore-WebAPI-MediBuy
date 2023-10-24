namespace MediBuyApi.Models.DTO
{
    public class CartDTO
    {
        public ICollection<CartDetailDTO> CartDetails { get; set; }
    }
}
