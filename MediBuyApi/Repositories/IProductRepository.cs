namespace MediBuyApi.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(
            string? nameFilter,
            string? descriptionFilter,
            string? sellerFilter,
            int? lessThanPrice,
            int? greaterThanPrice,
            string sortBy = null,
            bool isAcending = true,
            int pageNumber = 1,
            int pageSize = 1000);

        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(int id, Product product);
        Task<Product> DeleteAsync(int id);
    }
}