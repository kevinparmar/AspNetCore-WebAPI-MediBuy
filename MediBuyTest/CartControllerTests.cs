using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediBuyApi.Controllers;
using MediBuyApi.Repositories;
using MediBuyApi.Models.DTO;
using MediBuyApi.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace MediBuyTest
{
    public class CartControllerTests
    {
        [Fact]
        public async Task GetAllItems_UserLoggedIn_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<ICartRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CartController(mockRepository.Object);

            var cartDTO = new CartDTO
            {
                CartDetails = new List<CartDetailDTO>
    {
        new CartDetailDTO
        {
            Id = 1,
            ProductId = 101, 
            Quantity = 3, 
            UnitPrice = 9.99, 
            Product = new ProductDTO
            {
                Id = 101,
                Name = "Product1",
                Price = 9.99,
                Seller = "Seller1",
                Description = "Description1",
                Availability = 10,
                CategoryId = 1,
                CategoryName = "Category1"
            }
        },
        new CartDetailDTO
        {
            Id = 2,
            ProductId = 102,
            Quantity = 2, 
            UnitPrice = 14.99, 
            Product = new ProductDTO
            {
                Id = 102,
                Name = "Product2",
                Price = 14.99,
                Seller = "Seller2",
                Description = "Description2",
                Availability = 5,
                CategoryId = 2,
                CategoryName = "Category2"
            }
        },
    }
            };

            mockRepository.Setup(repo => repo.GetUserCartAsync()).ReturnsAsync(cartDTO);

            // Act
            var result = await controller.GetAllItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<CartDTO>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        }

        [Fact]
        public async Task AddItem_ItemNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<ICartRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CartController(mockRepository.Object);

            var itemId = 1;
            mockRepository.Setup(repo => repo.AddItemAsync(itemId)).ReturnsAsync((CartDetailDTO)null);

            // Act
            var result = await controller.AddItem(itemId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            // Additional assertions as needed.
        }

        [Fact]
        public async Task RemoveItem_ItemFound_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<ICartRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CartController(mockRepository.Object);

            var itemId = 1;
            var cartDetailDTO = new CartDetailDTO
            {
                Id = itemId,
                // Other properties as needed
            };

            mockRepository.Setup(repo => repo.RemoveItemAsync(itemId)).ReturnsAsync(cartDetailDTO);

            // Act
            var result = await controller.RemoveItem(itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<CartDetailDTO>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            // Additional assertions as needed.
        }

        [Fact]
        public async Task RemoveItem_ItemNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<ICartRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CartController(mockRepository.Object);

            var itemId = 1;
            mockRepository.Setup(repo => repo.RemoveItemAsync(itemId)).ReturnsAsync((CartDetailDTO)null); // Simulate item not found.


            // Act
            var result = await controller.RemoveItem(itemId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            // Additional assertions as needed.
        }

        [Fact]
        public async Task ClearCart_Success_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<ICartRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CartController(mockRepository.Object);

            mockRepository.Setup(repo => repo.ClearCartAsync()).Verifiable();

            // Act
            var result = await controller.ClearCart();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            // Additional assertions as needed.
        }

        [Fact]
        public async Task Checkout_Success_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<ICartRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CartController(mockRepository.Object);

            var orderDTO = new OrderDTO
            {
                // Initialize with data as needed
            };

            mockRepository.Setup(repo => repo.CheckoutAsync()).ReturnsAsync(orderDTO);

            // Act
            var result = await controller.Checkout();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<OrderDTO>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            // Additional assertions as needed.
        }

        [Fact]
        public async Task Checkout_EmptyCart_ReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<ICartRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CartController(mockRepository.Object);

            mockRepository.Setup(repo => repo.CheckoutAsync()).ReturnsAsync((OrderDTO)null); // Simulate an empty cart.


            // Act
            var result = await controller.Checkout();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            // Additional assertions as needed.
        }

        [Fact]
        public async Task Checkout_Exception_ReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<ICartRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CartController(mockRepository.Object);

            mockRepository.Setup(repo => repo.CheckoutAsync()).Throws(new Exception("Simulated error"));

            // Act
            var result = await controller.Checkout();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            // Additional assertions as needed.
        }
    }
}
