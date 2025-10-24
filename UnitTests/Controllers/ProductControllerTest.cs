using Catalog.Controllers;
using Catalog.Entity;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using APP.Features.Products.Queries;
using APP.Features.Products.Commands;
using APP.DTO;
using APP.Interfaces;


namespace UnitTests.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductController _controller;
        private readonly Mock<ILogger<ProductController>> logger;
        private readonly Mock<IMediator> _mockMediator;

        public ProductControllerTest() 
        {
            _mockProductService = new Mock<IProductService>();
             logger = new Mock<ILogger<ProductController>>();
            _mockMediator = new Mock<IMediator>();
            _controller = new ProductController(_mockProductService.Object, logger.Object, _mockMediator.Object);
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

            //_mockProductService.Setup(s => s.GetAllProductsAsync())
                //.ReturnsAsync(products);

            _mockMediator.Setup(m => m.Send(new GetAllProductsQuery(), It.IsAny<CancellationToken>()))
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
            //_mockProductService.Setup(s => s.GetAllProductsAsync())
              //  .ReturnsAsync(Enumerable.Empty<ProductDTO>);

            _mockMediator.Setup(m => m.Send(new GetAllProductsQuery(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<ProductDTO>());

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

            //_mockProductService.Setup(s => s.DeleteProductByIdAsync(DeleteProductId))
              //  .ReturnsAsync(true);

            _mockMediator.Setup(m => m.Send(new DeleteProductCommand(DeleteProductId), It.IsAny<CancellationToken>()))
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

            //_mockProductService.Setup(s => s.DeleteProductByIdAsync(notExistProductId))
              //  .ReturnsAsync(false);

            _mockMediator.Setup(m => m.Send(new DeleteProductCommand(notExistProductId), It.IsAny<CancellationToken>()))
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
            var product = new CreateProductCommand
            (
                Name: "CNN",
                Description: "Information",
                Price: 10.0m,
                CategoryId: 1
            );

            _mockMediator.Setup(s => s.Send(product, It.IsAny<CancellationToken>()))
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
            CreateProductCommand? product = null!;

            _mockMediator.Setup(s => s.Send(product, It.IsAny<CancellationToken>()))
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
            var product = new UpdateProductCommand
            (
                Id: 1,
                Name: "Updated Product",
                Description: "Updated Description",
                Price: 15.0m,
                CategoryId: 1
            );

            _mockMediator.Setup(s => s.Send(product, It.IsAny<CancellationToken>()))
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

            //_mockProductService.Setup(s => s.GetProductByIdAsync(productId))
              //  .ReturnsAsync(product);

            _mockMediator.Setup(m => m.Send(new GetProductByIdQuery(productId), It.IsAny<CancellationToken>()))
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

        [Fact]
        public async Task GetProductByIdWithCategory_ShouldReturnProduct()
        {
            //Arrange 
            var productId = 1;
            var product = new ProductWithCategoryDTO
            {
                Id = productId,
                Name = "Iphone 17",
                Description = "Newly iphone with IA",
                Price = 1000,
                Category = new CategoryDTO
                {
                    Id = 1,
                    Name = "Tech",
                    Description = "All about tech"
                }
            };

            //_mockProductService.Setup(s => s.GetProductByIdWithCategoryDTO(productId))
                //.ReturnsAsync(product);

            _mockMediator.Setup(m => m.Send(new GetProductWithCategoryQuery(productId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            //Act 
            var result = await _controller.GetProductByIdWithCategory(productId);

            //Assert
            var resultOk = result.Result.Should().BeOfType<OkObjectResult>().Which;
            var response = resultOk.Value.Should().BeOfType<ApiResponse<ProductWithCategoryDTO>>().Which;

            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
            var productResult = response.Data.Should().BeOfType<ProductWithCategoryDTO>().Which;
            productResult.Id.Should().Be(productId);    
        }
    }
}
