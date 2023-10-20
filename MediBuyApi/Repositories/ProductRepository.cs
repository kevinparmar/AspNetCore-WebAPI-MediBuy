using MediBuyApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace MediBuyApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MediBuyDbContext dbContext;

        public ProductRepository(MediBuyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Product>> GetAllAsync(
            string? nameFilter,
            string? descriptionFilter,
            string? sellerFilter,
            int? lessThanPrice,
            int? greaterThanPrice,
            string sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            var products = dbContext.Products.AsQueryable();

            //Filter by Name

            if(!string.IsNullOrEmpty(nameFilter))
            {
                products = products.Where(p => p.Name.Contains(nameFilter));
            }

            //Filter by description

            if (!string.IsNullOrEmpty(descriptionFilter))
            {
                products = products.Where(p => p.Description.Contains(descriptionFilter));
            }

            //Filter by seller

            if (!string.IsNullOrEmpty(sellerFilter))
            {
                products = products.Where(p => p.Seller.Contains(sellerFilter));
            }

            //Filter by price (less than)

            if (lessThanPrice != null)
            {
                products = products.Where(p => p.Price < lessThanPrice);
            }

            //Filter by price (greater than)

            if (greaterThanPrice != null)
            {
                products = products.Where(p => p.Price > greaterThanPrice);
            }

            //Sorting

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                //Sort by name
                
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Name) : products.OrderByDescending(x => x.Name);
                }

                //Sort by price 

                else if (sortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Price) : products.OrderByDescending(x => x.Price);
                }
            }

            var skipResults = (pageNumber - 1) * pageSize;

            return await products.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(int id, Product product)
        {
            var existingProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Image = product.Image;
            existingProduct.Seller = product.Seller;
            existingProduct.Description = product.Description;
            existingProduct.Availability = product.Availability;
            existingProduct.CategoryId = product.CategoryId;

            await dbContext.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var existingProduct = dbContext.Products.FirstOrDefault(p => p.Id == id);

            if(existingProduct == null)
            {
                return null;
            }

            dbContext.Products.Remove(existingProduct);
            await dbContext.SaveChangesAsync();
            return existingProduct;
        }
    }
}
