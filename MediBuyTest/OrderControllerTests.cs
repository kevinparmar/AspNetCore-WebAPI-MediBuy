using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediBuyApi.Controllers;
using MediBuyApi.Models.DTO;
using MediBuyApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MediBuyTest
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOkWithOrders()
        {
            // Arrange
            var mockRepository = new Mock<IOrderRepository>();
            var controller = new OrderController(mockRepository.Object);

            var ordersDTO = new List<OrderDTO>
        {
            new OrderDTO { Id = 1, CreateDate = DateTime.UtcNow, OrderStatusId = 1, OrderStatusName = "Pending", OrderDetails = new List<OrderDetailDTO>() },
            // Add more orders as needed
        };

            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(ordersDTO);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<OrderDTO>>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotEmpty(model);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFoundWhenNoOrders()
        {
            // Arrange
            var mockRepository = new Mock<IOrderRepository>();
            var controller = new OrderController(mockRepository.Object);

            // Simulate no orders found
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync((List<OrderDTO>)null);

            // Act
            var result = await controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task StatusUpdate_ReturnsOkWithUpdatedOrder()
        {
            // Arrange
            var mockRepository = new Mock<IOrderRepository>();
            var controller = new OrderController(mockRepository.Object);

            var orderId = 1;
            var statusUpdateDTO = new StatusUpdateDTO { OrderStatusId = 2 };
            var orderDTO = new OrderDTO { Id = orderId, CreateDate = DateTime.UtcNow, OrderStatusId = statusUpdateDTO.OrderStatusId, OrderStatusName = "UpdatedStatus", OrderDetails = new List<OrderDetailDTO>() };

            mockRepository.Setup(repo => repo.StatusUpdateAsync(orderId, statusUpdateDTO.OrderStatusId)).ReturnsAsync(orderDTO);

            // Act
            var result = await controller.StatusUpdate(orderId, statusUpdateDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<OrderDTO>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(orderId, model.Id);
            Assert.Equal(statusUpdateDTO.OrderStatusId, model.OrderStatusId);
        }
        [Fact]
        public async Task StatusUpdate_ReturnsNotFoundWhenOrderNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IOrderRepository>();
            var controller = new OrderController(mockRepository.Object);

            var orderId = 1;
            var statusUpdateDTO = new StatusUpdateDTO { OrderStatusId = 2 };

            // Simulate no such order found
            mockRepository.Setup(repo => repo.StatusUpdateAsync(orderId, statusUpdateDTO.OrderStatusId)).ReturnsAsync((OrderDTO)null);


            // Act
            var result = await controller.StatusUpdate(orderId, statusUpdateDTO);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
    }
}
