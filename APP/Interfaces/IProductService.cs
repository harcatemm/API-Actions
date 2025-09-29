using APP.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO?>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(ProductDTO? productDto);        
        Task<bool> DeleteProductByIdAsync(int id);
        Task<bool> UpdateProductByIdAsync(ProductDTO? productDto);
        Task<ProductWithCategoryDTO?> GetProductByIdWithCategoryDTO(int id);
    }
}
