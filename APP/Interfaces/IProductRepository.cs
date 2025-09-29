using APP.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product?>> GetTopExpensiveAsync(int top, CancellationToken ct = default);
        Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken ct = default);        
    }
}
