namespace MediBuyApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> Create(Category category);
        Task<Category> UpdateAsync(int id, Category category);
        Task<Category> DeleteAsync(int id);
    }
}