using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using IntegrationTests.Configuration;
using APP.DTO;
using Domain.Entities;
using Catalog.Entity;

namespace IntegrationTests.Controllers
{
    public class ProductControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        public ProductControllerTest(CustomWebApplicationFactory factory) 
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnProducts_WhenExist()
        {
            //Arrange

            //Act
            var result = await _httpClient.GetAsync("api/v1/Product");

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await result.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<ProductDTO?>>>();
            response?.Success.Should().BeTrue();
            response?.Data.Should().NotBeNull();
            var data = response?.Data.Should().BeAssignableTo<IEnumerable<ProductDTO?>>().Which;
            data.Should().HaveCount(4);            
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenIdProductExist()
        {
            //Arrange
            var productId = 1;

            //Act
            var result = await _httpClient.GetAsync($"api/v1/Product/{productId}");

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await result.Content.ReadFromJsonAsync<ApiResponse<ProductDTO?>>();
            response?.Success.Should().BeTrue();
            response?.Data.Should().NotBeNull();
            var data = response?.Data.Should().BeAssignableTo<ProductDTO?>().Which;
            data?.Id.Should().Be(productId);
        }
    }
}
