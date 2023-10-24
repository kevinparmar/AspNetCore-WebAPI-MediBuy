namespace MediBuyApi.Repositories
{
    public interface ICartRepository
    {
        Task<CartDTO> GetUserCartAsync();
        Task<CartDetailDTO> AddItemAsync(int id);
        Task<CartDetailDTO> RemoveItemAsync(int id);
        Task ClearCartAsync();
        Task<OrderDTO> CheckoutAsync();
        Task<int> GetCartItemCountAsync();
    }
}