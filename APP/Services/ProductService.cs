using APP.DTO;
using APP.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        public ProductService(IUnitOfWork uow) 
        {
            this._uow = uow;
        }
        public async Task<bool> AddProductAsync(ProductDTO? productDto)
        {
            if (productDto is null)
                return false;

            var product = await _uow.Products.GetByIdAsync(productDto.Id);
            if (product is null)
            {
                var newProduct = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Description = productDto.Description,
                    CategoryId = productDto.CategoryId
                };
                await _uow.Products.AddAsync(newProduct);
                await _uow.CommitAsync();
                return true;
            }
            else
                return false;            
        }
        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product is not null)
            {
                _uow.Products.Delete(product);
                await _uow.CommitAsync();
                return true;
            }
            else
                return false;            
        }
        public async Task<bool> UpdateProductByIdAsync(ProductDTO product)
        {
            if(product is null)
                return false;

            var existingProduct = await _uow.Products.GetByIdAsync(product.Id);
            if (existingProduct is not null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                _uow.Products.Update(existingProduct);
                await _uow.CommitAsync();
                return true;
            }
            else
                return false;        
        }
        public async Task<IEnumerable<ProductDTO?>> GetAllProductsAsync()
        {
            var products = await _uow.Products.GetAllAsync();
            if (products.Any())
                return products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryId = p.CategoryId
                });
            else
                return Enumerable.Empty<ProductDTO>();            
        }        

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product is not null)
                return new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CategoryId = product.CategoryId
                };
            else
                return null;
        }

        public async Task<ProductWithCategoryDTO?> GetProductByIdWithCategoryDTO(int id)
        {
            var product = await _uow.Products.GetByIdWithCategoryAsync(id);
            if (product is not null)
                return new ProductWithCategoryDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Category = new CategoryDTO
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name,
                        Description = product.Category.Description
                    }
                };
            else
                return null;
        }
    }
}
