using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediBuyApi.Controllers;
using MediBuyApi.Models.DTO;
using MediBuyApi.Models.Domain;
using MediBuyApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MediBuyTest
{
    public class CategoryControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsAllCategories()
        {
            // Arrange
            var mockRepository = new Mock<ICategoryRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CategoryController(mockRepository.Object, mockMapper.Object);

            var categoryList = new List<CategoryDTO>
        {
            new CategoryDTO { Id = 1, CategoryName = "Category1" },
            new CategoryDTO { Id = 2, CategoryName = "Category2" }
        };

            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categoryList);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CategoryDTO>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsCategory()
        {
            // Arrange
            var mockRepository = new Mock<ICategoryRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CategoryController(mockRepository.Object, mockMapper.Object);

            var categoryId = 1;
            var categoryDTO = new CategoryDTO { Id = categoryId, CategoryName = "Category1" };

            mockRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(categoryDTO);

            // Act
            var result = await controller.GetById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(categoryId, model.Id);
            Assert.Equal("Category1", model.CategoryName);
        }

        [Fact]
        public async Task Create_ValidCategory_ReturnsCreated()
        {
            // Arrange
            var mockRepository = new Mock<ICategoryRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CategoryController(mockRepository.Object, mockMapper.Object);

            var editCategoryDTO = new EditCategoryDTO { CategoryName = "NewCategory" };
            var categoryDomain = new Category { Id = 1, CategoryName = "NewCategory" };
            var categoryDTO = new CategoryDTO { Id = 1, CategoryName = "NewCategory" };

            // Configure the mapper to map from EditCategoryDTO to Category.
            mockMapper.Setup(mapper => mapper.Map<Category>(editCategoryDTO)).Returns(categoryDomain);

            // Configure the repository to return the created categoryDomain.
            mockRepository.Setup(repo => repo.Create(It.IsAny<Category>())).ReturnsAsync(categoryDomain);

            // Configure the mapper to map from Category to CategoryDTO.
            mockMapper.Setup(mapper => mapper.Map<CategoryDTO>(categoryDomain)).Returns(categoryDTO);

            // Act
            var result = await controller.Create(editCategoryDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<CategoryDTO>(createdResult.Value);

            // Assert the status code, it should be 201 (Created).
            Assert.Equal(201, createdResult.StatusCode);

            // Assert the location header.
            Assert.Equal(nameof(CategoryController.GetById), createdResult.ActionName);
            Assert.Equal(categoryDomain.Id, createdResult.RouteValues["id"]);

            // Assert the category name in the returned CategoryDTO.
            Assert.Equal("NewCategory", model.CategoryName);
        }

        [Fact]
        public async Task Update_ExistingId_ValidCategory_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<ICategoryRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CategoryController(mockRepository.Object, mockMapper.Object);

            var categoryId = 1;
            var editCategoryDTO = new EditCategoryDTO { CategoryName = "UpdatedCategory" };
            var categoryDomain = new Category { Id = categoryId, CategoryName = "UpdatedCategory" };
            var updatedCategoryDomain = new Category { Id = categoryId, CategoryName = "UpdatedCategory" }; // Assume the repository returns the updated object.

            mockMapper.Setup(mapper => mapper.Map<Category>(editCategoryDTO)).Returns(categoryDomain);
            mockRepository.Setup(repo => repo.UpdateAsync(categoryId, categoryDomain)).ReturnsAsync(updatedCategoryDomain); // Simulate the update.
            mockMapper.Setup(mapper => mapper.Map<CategoryDTO>(updatedCategoryDomain)).Returns(new CategoryDTO { Id = categoryId, CategoryName = "UpdatedCategory" }); // Map to CategoryDTO.

            // Act
            var result = await controller.Update(categoryId, editCategoryDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("UpdatedCategory", model.CategoryName);
        }


        [Fact]
        public async Task Delete_ExistingId_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<ICategoryRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CategoryController(mockRepository.Object, mockMapper.Object);

            var categoryId = 1;
            var categoryDomain = new Category { Id = categoryId, CategoryName = "CategoryToDelete" };

            mockRepository.Setup(repo => repo.DeleteAsync(categoryId)).ReturnsAsync(categoryDomain);
            mockMapper.Setup(mapper => mapper.Map<CategoryDTO>(categoryDomain)).Returns(new CategoryDTO { Id = categoryId, CategoryName = "CategoryToDelete" }); // Map to CategoryDTO.

            // Act
            var result = await controller.Delete(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("CategoryToDelete", model.CategoryName);
        }


    }
}