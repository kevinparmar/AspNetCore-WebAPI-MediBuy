using MediBuyApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace MediBuyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        [Authorize(Roles = "Reader")]
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var cartDTO = await cartRepository.GetUserCartAsync();

            if (cartDTO == null)
            {
                return Unauthorized(new { Message = "User not logged in" });
            }
            else if (cartDTO.CartDetails.Count == 0)
            {
                return NoContent();
            }

            return Ok(cartDTO);
        }

        [Authorize(Roles = "Reader")]
        [HttpPost]
        [Route("{Id:int}")]
        public async Task<IActionResult> AddItem(int Id)
        {
            var cartDetailDTO = await cartRepository.AddItemAsync(Id);

            if (cartDetailDTO == null)
            {
                return NotFound(new { Message = "Item not found" });
            }

            return Ok(cartDetailDTO);
        }

        [Authorize(Roles = "Reader")]
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> RemoveItem(int Id)
        {
            var cartDetailDTO = await cartRepository.RemoveItemAsync(Id);

            if (cartDetailDTO == null)
            {
                return NotFound(new { Message = "Item not found" });
            }

            return Ok(cartDetailDTO);
        }

        [Authorize(Roles = "Reader")]
        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                await cartRepository.ClearCartAsync();
                return Ok("Cart cleared successfully");
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed.
                return BadRequest("Error clearing the cart");
            }
        }

        [Authorize(Roles = "Reader")]
        [HttpGet]
        [Route("api/Cart/GetItemsCount")]
        public async Task<IActionResult> GetItemsCount()
        {
            var count = await cartRepository.GetCartItemCountAsync();
            return Ok(new { Message = $"Total items in the cart: {count}" });
        }

        [Authorize(Roles = "Reader")]
        [HttpGet]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var orderDTO = await cartRepository.CheckoutAsync(); // Assuming your repository method returns OrderDTO

                if (orderDTO == null)
                {
                    return NotFound(new { Message = "Cart is empty" });
                }

                return Ok(orderDTO);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed.
                return BadRequest("Error processing the order");
            }
        }
    }


}
