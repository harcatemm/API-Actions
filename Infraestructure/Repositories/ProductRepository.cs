using APP.Interfaces;
using Domain.Entities;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Infraestructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        public ProductRepository(AppDbContext appDb) : base(appDb)
        {
            context = appDb;
        }
        public async Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken ct = default)
        {
            return await context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(ct);                
        }

        public async Task<IEnumerable<Product?>> GetTopExpensiveAsync(int top, CancellationToken ct = default)
        {
            return await context.Products
                 .OrderByDescending(p => p.Price)
                 .Take(top)
                 .ToListAsync(ct);
        }
    }
}
