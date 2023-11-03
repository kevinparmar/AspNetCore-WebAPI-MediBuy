using MediBuyApi.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MediBuyApi.CustomActionFilters;

namespace MediBuyApi.Repositories
{
    [Authorize(Roles = "Reader")]
    public class CartRepository : ICartRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly MediBuyDbContext dbContext;

        public CartRepository(MediBuyDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<CartDetailDTO> AddItemAsync(int id)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await GetCart();
                var cartDetail = await dbContext.CartDetails
                                                .Include(cd => cd.Product)
                                                .ThenInclude(p => p.Category)
                                                .FirstOrDefaultAsync(c => c.CartId == cart.Id && c.ProductId == id);

                if (cartDetail == null)
                {
                    var product = await dbContext.Products
                                                 .Include(p => p.Category)
                                                 .FirstOrDefaultAsync(p => p.Id == id);
                    cartDetail = new CartDetail
                    {
                        CartId = cart.Id,
                        ProductId = product.Id,
                        Quantity = 1,
                        UnitPrice = product.Price
                    };
                    dbContext.CartDetails.Add(cartDetail);
                }
                else
                {
                    cartDetail.Quantity += 1;
                }

                await dbContext.SaveChangesAsync();
                transaction.Commit();

                return mapper.Map<CartDetailDTO>(cartDetail);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // Log or handle the exception as needed.
                throw;
            }
        }

        public async Task<CartDetailDTO> RemoveItemAsync(int id)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await GetCart();
                var cartDetail = await dbContext.CartDetails
                                                .Include(cd => cd.Product)
                                                .ThenInclude(p => p.Category)
                                                .FirstOrDefaultAsync(c => c.CartId == cart.Id && c.ProductId == id);

                if (cartDetail == null)
                {
                    return null;
                }
                else if (cartDetail.Quantity > 1)
                {
                    cartDetail.Quantity -= 1;
                }
                else
                {
                    dbContext.CartDetails.Remove(cartDetail);
                }

                await dbContext.SaveChangesAsync();
                transaction.Commit();

                return mapper.Map<CartDetailDTO>(cartDetail);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // Log or handle the exception as needed.
                throw;
            }
        }

        public async Task ClearCartAsync()
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await GetCart();
                if (cart != null)
                {
                    var cartDetails = await dbContext.CartDetails
                                                     .Where(cd => cd.CartId == cart.Id)
                                                     .ToListAsync();

                    dbContext.CartDetails.RemoveRange(cartDetails);
                    await dbContext.SaveChangesAsync();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // Log or handle the exception as needed.
                throw;
            }
        }
        public async Task<CartDTO> GetUserCartAsync()
        {
            var userId = await GetUserId();
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await dbContext.Carts
                    .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Product)
                    .ThenInclude(p => p.Category)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                    };

                    dbContext.Carts.Add(cart);
                    await dbContext.SaveChangesAsync();
                }

                transaction.Commit();

                var cartDTO = mapper.Map<CartDTO>(cart);
                return cartDTO;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // Log or handle the exception as needed.
                throw;
            }
        }

        public async Task<Cart> GetCart()
        {
            var userId = await GetUserId();
            var cart =  await dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                };

                dbContext.Carts.Add(cart);
                await dbContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<int> GetCartItemCountAsync()
        {
            var cart = await GetCart();
            var totalQuantity = await dbContext.CartDetails
                                               .Where(cartDetail => cartDetail.CartId == cart.Id)
                                               .SumAsync(cartDetail => cartDetail.Quantity);

            return totalQuantity;
        }

        [ValidateModel]
        public async Task<OrderDTO> CheckoutAsync()
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var userId = await GetUserId();
                var cart = await GetCart();
                var cartDetails = dbContext.CartDetails.Where(cd => cd.CartId == cart.Id);

                //var cartDetails = cart.CartDetails;

                if (!cartDetails.Any())
                {
                    // Handle an empty cart gracefully
                    throw new Exception("Cart is empty");
                }

                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1
                };

                dbContext.Orders.Add(order);
                await dbContext.SaveChangesAsync();

                foreach (var product in cartDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        UnitPrice = product.UnitPrice,
                    };
                    dbContext.OrderDetails.Add(orderDetail);
                }
                await dbContext.SaveChangesAsync();

                dbContext.CartDetails.RemoveRange(cartDetails);
                await dbContext.SaveChangesAsync();

                order = dbContext.Orders
                                 .Include(o => o.OrderStatus)
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(o => o.Product)
                                 .ThenInclude(o => o.Category)
                                 .FirstOrDefault(o => o.Id == order.Id);

                transaction.Commit();

                // After committing the transaction, map the Order to OrderDTO
                var orderDTO = mapper.Map<OrderDTO>(order);

                return orderDTO;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // Log or handle the exception as needed.
                throw;
            }
        }

        [ValidateModel]
        public async Task<string> GetUserId()
        {
            var userClaims = httpContextAccessor.HttpContext.User.Claims;

            var emailClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim != null)
            {
                string email = emailClaim.Value;
                var user = await userManager.FindByEmailAsync(email);
                return user.Id;
            }

            //throw new Exception("User not logged in or incorrect token passed!");
            httpContextAccessor.HttpContext.Response.StatusCode = 401; // Set the HTTP status code to 401
            return "User not logged in or incorrect token passed!";
        }
    }
}
