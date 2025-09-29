using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using FluentAssertions;
using APP.Interfaces;
using APP.DTO;
using Catalog.Controllers;
using Microsoft.AspNetCore.Mvc;
using Catalog.Entity;
using Microsoft.Extensions.DependencyInjection;


namespace UnitTests.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductController _controller;
        public ProductControllerTest() 
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnProducts()
        {
            // Arrange
            var products = new List<ProductDTO>
            {
                new ProductDTO { Id = 1, Name = "Product1", Price = 10.0m, Description = "", CategoryId = 1 },
                new ProductDTO { Id = 2, Name = "Product2", Price = 20.0m, Description = "", CategoryId = 1 }
            };

            var apiResponse = new ApiResponse<IEnumerable<ProductDTO?>>
            {
                Success = true,
                Message = $"{products.Count()} products found",
                Data = products
            };

            _mockProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.Get();

            // Assert
            var resultOk = result.Result.Should().BeOfType<OkObjectResult>().Which;
            var response = resultOk.Value.Should().BeOfType<ApiResponse<IEnumerable<ProductDTO?>>>().Which;

            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Data.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnNotFound_WhenNoProducts()
        {
            // Arrange
            _mockProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(Enumerable.Empty<ProductDTO>);

            var expectedResponse = new ApiResponse<IEnumerable<ProductDTO?>>
            {
                Success = false,
                Message = "No products found",
                Data = null
            };
            // Act
            var result = await _controller.Get();

            // Assert
            var resultOk = result.Result.Should().BeOfType<NotFoundObjectResult>().Which;
            var response = resultOk.Value.Should().BeOfType<ApiResponse<IEnumerable<ProductDTO?>>>().Which;

            response.Success.Should().BeFalse();
            response.Message.Should().Be("No products found");
            response.Data.Should().BeNull();
        }

        [Fact]
        public async Task DeleteProductById_ShouldReturnOk_WhenProductFound()
        {
            //arrange 
            int DeleteProductId = 1;
            _mockProductService.Setup(s => s.DeleteProductByIdAsync(DeleteProductId))
                .ReturnsAsync(true);

            var apiResponse = new ApiResponse<bool>
            {
                Success = true,
                Message = "Product deleted successfully",
                Data = true
            };

            //Act
            var result = await _controller.Delete(DeleteProductId);

            //Assert
            var resultOk = result.Result.Should().BeOfType<OkObjectResult>().Which;
            var response = resultOk.Value.Should().BeOfType<ApiResponse<bool>>().Which;

            response.Success.Should().BeTrue();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteProductById_ShouldReturnFalse_WhenNotFound()
        {
            //Arrange
            int notExistProductId = 99;
            _mockProductService.Setup(s => s.DeleteProductByIdAsync(notExistProductId))
                .ReturnsAsync(false);

            var apiResponse = new ApiResponse<bool>
            {
                Success = false,
                Message = "Id product not found",
                Data = false
            };

            //Act
            var result = await _controller.Delete(notExistProductId);

            //Assert
            var resultOk = result.Result.Should().BeOfType<NotFoundObjectResult>().Which;
            var response = resultOk.Value.Should().BeOfType<ApiResponse<bool>>().Which;

            response.Success.Should().BeFalse();
            response.Data.Should().BeFalse();
            response.Message.Should().Be("Id product not found");
        }

        [Fact]
        public async Task AddProduct_ShouldReturnOk_WhenProductIsValid()
        {
            // Arrange
            var product = new ProductDTO
            {
                Name = "CNN",
                Description = "Information",
                Price = 10.0m,
                CategoryId = 1
            };

            _mockProductService.Setup(s => s.AddProductAsync(product))
                .ReturnsAsync(true);

            //Act
            var result = await _controller.Add(product);

            //Assert
            var resoltOk = result.Result.Should().BeOfType<OkObjectResult>().Which;
            var response = resoltOk.Value.Should().BeOfType<ApiResponse<bool>>().Which;

            response.Success.Should().BeTrue();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task AddProduct_ShouldReturnFalse_WhenProductIsInvalid()
        {
            //Arrenge
            ProductDTO? product = null;

            _mockProductService.Setup(s => s.AddProductAsync(product))
                .ReturnsAsync(false);

            //Act
            var result = await _controller.Add(product);

            //Assert
            var resultOk = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
            var response = resultOk.Value.Should().BeOfType<ApiResponse<bool>>().Which;

            response.Success.Should().BeFalse();
            response.Data.Should().BeFalse();
            response.Message.Should().Be("Product is null");
        }

        [Fact]
        public async Task UpdateProductById_ShouldReturnOk_WhenProductIsValid()
        {
            // Arrange
            var updateProductId = 1;
            var product = new ProductDTO
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 15.0m,
                CategoryId = 1
            };

            _mockProductService.Setup(s => s.UpdateProductByIdAsync(product))
                .ReturnsAsync(true);

            //Act 
            var result = await _controller.Update(updateProductId, product);

            //Assert
            var resultOk = result.Result.Should().BeOfType<OkObjectResult>().Which;
            var response = resultOk.Value.Should().BeOfType<ApiResponse<bool>>().Which;

            response.Success.Should().BeTrue();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOk_WhenFoundId()
        {
            //Arrange
            var productId = 1;
            var product = new ProductDTO
            {
                Id = 1,
                Name = "Oppo 13 X2",
                Description = "Tech",
                Price = 1000,
                CategoryId = 1
            };

            _mockProductService.Setup(s => s.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            //Act
            var result = await _controller.GetProductById(productId);

            //Assert
            var resultOk = result.Result.Should().BeOfType<OkObjectResult>().Which;
            var response = resultOk.Value.Should().BeOfType<ApiResponse<ProductDTO?>>().Which;

            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
            var productResult = response.Data.Should().BeOfType<ProductDTO>().Which;
            productResult.Id.Should().Be(productId);
            productResult.Name.Should().Be(product.Name);



        }
    }
}
