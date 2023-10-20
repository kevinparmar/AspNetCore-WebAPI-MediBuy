using MediBuyApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace MediBuyApi.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly MediBuyDbContext dbContext;

        public CategoryRepository(MediBuyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> Create(Category category) 
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(int id, Category category)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(existingCategory == null)
            {
                return null;
            }

            existingCategory.CategoryName = category.CategoryName;
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<Category> DeleteAsync(int id)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCategory == null)
            {
                return null;
            }

            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }
    }
}
