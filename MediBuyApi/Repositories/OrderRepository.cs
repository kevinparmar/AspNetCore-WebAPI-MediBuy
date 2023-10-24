using AutoMapper;
using MediBuyApi.CustomActionFilters;
using MediBuyApi.Data;
using System.Security.Claims;

namespace MediBuyApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MediBuyDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public OrderRepository(MediBuyDbContext dbContext, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }

        public async Task<List<OrderDTO>> GetAllAsync()
        {
            var userId = await GetUserId();
            var orders = await dbContext.Orders
                                        .Include(o => o.OrderStatus)
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(o => o.Product)
                                        .ThenInclude(o => o.Category)
                                        .ToListAsync();

            var ordersDTO = mapper.Map<List<OrderDTO>>(orders);
            return ordersDTO;
        }

        [ValidateModel]
        public async Task<OrderDTO> StatusUpdateAsync(int orderId, int statusId)
        {
            var userId = await GetUserId();
            var order = await dbContext.Orders
                                       .Where(o => o.Id == orderId)
                                       .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new Exception("No such order exists!");
            }
            
            order.OrderStatusId = statusId;
            await dbContext.SaveChangesAsync();

            order = await dbContext.Orders
                                   .Include(o => o.OrderStatus)
                                   .Include(o => o.OrderDetails)
                                   .ThenInclude(o => o.Product)
                                   .ThenInclude(o => o.Category)
                                   .Where(o => o.Id == orderId)
                                   .FirstOrDefaultAsync();

            var orderDTO = mapper.Map<OrderDTO>(order);

            return orderDTO;
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

            throw new Exception("User not logged in or incorrect token passed!");
        }
    }
}
