using MediBuyApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace MediBuyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ordersDTO = await orderRepository.GetAllAsync();
            if(ordersDTO == null)
            {
                return NotFound(new {Message = "No orders found"});
            }
            return Ok(ordersDTO);
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> StatusUpdate(int Id, [FromBody] StatusUpdateDTO statusUpdateDTO)
        {
            var orderDTO = await orderRepository.StatusUpdateAsync(Id, statusUpdateDTO.OrderStatusId);

            if(orderDTO == null)
            {
                return NotFound(new { Message = "No such order found" });
            }
            return Ok(orderDTO);
        }
    }
}
