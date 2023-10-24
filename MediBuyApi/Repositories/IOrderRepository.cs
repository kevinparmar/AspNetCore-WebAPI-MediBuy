namespace MediBuyApi.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDTO>> GetAllAsync();
        Task<OrderDTO> StatusUpdateAsync(int orderId, int statusId);
    }
}