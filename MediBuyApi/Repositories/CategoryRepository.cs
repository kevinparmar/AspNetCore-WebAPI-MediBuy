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

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var category = await dbContext.Categories
                                          .Select(c => new CategoryDTO
                                          {
                                              Id = c.Id,
                                              CategoryName = c.CategoryName,
                                              Products = c.Products.Select(p => new ProductWithoutCategory
                                              {
                                                  Id = p.Id,
                                                  Name = p.Name,
                                                  Price = p.Price,
                                                  Image = p.Image,
                                                  Seller = p.Seller,
                                                  Description = p.Description,
                                                  Availability = p.Availability,
                                                  CategoryId = p.CategoryId
                                              }).ToList()
                                              }).ToListAsync();

            return category;
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var categoryDTO = await dbContext.Categories
                                             .Where(c => c.Id == id)
                                             .Select(c => new CategoryDTO
                                             {
                                                 Id = c.Id,
                                                 CategoryName = c.CategoryName,
                                                 Products = c.Products.Select(p => new ProductWithoutCategory
                                                 {
                                                     Id = p.Id,
                                                     Name = p.Name,
                                                     Price = p.Price,
                                                     Image = p.Image,
                                                     Seller = p.Seller,
                                                     Description = p.Description,
                                                     Availability = p.Availability,
                                                     CategoryId = p.CategoryId,
                                                 }).ToList()
                                             })
                                             .FirstOrDefaultAsync();

            return categoryDTO;
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
