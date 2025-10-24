using APP.DTO;
using APP.Features.Products.Commands;
using APP.Features.Products.Queries;
using APP.Interfaces;
using Asp.Versioning;
using Catalog.Entity;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace Catalog.Controllers
{
    /// <summary>
    /// Product Endpoints
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService productService;
        private readonly IMediator _mediator;
        public ProductController(IProductService product, ILogger<ProductController> logger, IMediator mediator)
        {
            this.productService = product;
            this._logger = logger;
            this._mediator = mediator;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = "GetAllProducts")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDTO?>>>> Get()
        {
            _logger.LogInformation("{Controller}/Get method starts on {Time}",
                nameof(ProductController),
                DateTime.UtcNow);

            var products = await _mediator.Send(new GetAllProductsQuery());
            //var products = await productService.GetAllProductsAsync();
            if (products is null || !products.Any())
                return NotFound(new ApiResponse<IEnumerable<ProductDTO?>?>
                {
                    Success = false,
                    Message = "No products found",
                    Data = null
                });

            _logger.LogInformation("{Controller}/Get method finishes on {Time}",
                nameof(ProductController),
                DateTime.UtcNow);

            return Ok(new ApiResponse<IEnumerable<ProductDTO?>?>
            {
                Success = true,
                Message = $"{products.Count()} products found",
                Data = products
            });
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("", Name = "AddProduct")]
        public async Task<ActionResult<ApiResponse<bool>>> Add([FromBody] CreateProductCommand? product)
        {
            _logger.LogInformation("{Controller}/Add method starts on {Time}, with product data => {data}",
                nameof(ProductController),
                DateTime.UtcNow,
                JsonSerializer.Serialize(product));

            if (product is null)
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Product is null",
                    Data = false
                });

            var result = await _mediator.Send(product);
            //var result = await productService.AddProductAsync(product);
            if (result)
                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Product added successfully",
                    Data = true
                });


            _logger.LogInformation("{Controller}/Add method finishes on {Time}",
                nameof(ProductController),
                DateTime.UtcNow);

            return Conflict(new ApiResponse<bool>
            {
                Success = false,
                Message = "Product cannot be added",
                Data = false
            });
        }

        /// <summary>
        /// Delete a product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteProductById")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Invalid product id",
                    Data = false
                });

            var result = await _mediator.Send(new DeleteProductCommand(id));            
            //var result = await productService.DeleteProductByIdAsync(id);
            
            if (result)
                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Product deleted successfully",
                    Data = true
                });
            else
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Id product not found",
                    Data = false
                });
        }

        /// <summary>
        /// Update a product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id:int}", Name = "UpdateProductById")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] UpdateProductCommand product)
        {
            if (product is null || id != product.Id)
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Invalid product object",
                    Data = false
                });

            var result = await _mediator.Send(product);
            //var result = await productService.UpdateProductByIdAsync(product);
            if (result)
                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Product updated successfully",
                    Data = true
                });
            else
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Id product not found",
                    Data = false
                });
        }

        /// <summary>
        /// Get a product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<ApiResponse<ProductDTO?>>> GetProductById(int id)
        {
            if(id <= 0)
                return BadRequest(new ApiResponse<ProductDTO?>
                {
                    Success = false,
                    Message = "Invalid product id",
                    Data = null
                });

            var product = await _mediator.Send(new GetProductByIdQuery(id));
            //var product = await productService.GetProductByIdAsync(id);

            if (product is not null)
                return Ok(new ApiResponse<ProductDTO?>
                {
                    Success = true,
                    Message = "Product found",
                    Data = product
                });
            else 
                return NotFound(new ApiResponse<ProductDTO?>
                {
                    Success = false,
                    Message = "Id product not found",
                    Data = null
                });

        }

        /// <summary>
        /// Get a product by id with category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProductByIdWithCategory/{id:int}", Name = "GetProductByIdWithCategory")]
        public async Task<ActionResult<ApiResponse<ProductWithCategoryDTO>>> GetProductByIdWithCategory(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse<ProductWithCategoryDTO?>
                {
                    Success = false,
                    Message = "Invalid product id",
                    Data = null
                });

            var product = await _mediator.Send(new GetProductWithCategoryQuery(id));
            //var product = await productService.GetProductByIdWithCategoryDTO(id);

            if (product is not null)
                return Ok(new ApiResponse<ProductWithCategoryDTO?>
                {
                    Success = true,
                    Message = "Product found",
                    Data = product
                });
            else
                return NotFound(new ApiResponse<ProductWithCategoryDTO?>
                {
                    Success = false,
                    Message = "Id product not found",
                    Data = null
                });
        }
    }
}
