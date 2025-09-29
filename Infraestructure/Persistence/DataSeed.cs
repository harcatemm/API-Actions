using Domain.Entities;
using Infraestructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Persistence
{
    public static class DataSeed
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            await dbContext.Database.EnsureCreatedAsync();

            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Electronics", Description = "Electronic devices and gadgets" },
                    new Category { Name = "Books", Description = "Various genres of books" },
                    new Category { Name = "Clothing", Description = "Apparel and accessories" }
                };

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.Products.AnyAsync())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Iphone 13", Description = "Tech", Price = 1000, CategoryId = 1 },
                    new Product { Name = "The Great Gatsby", Description = "Classic novel", Price = 15, CategoryId = 2 },
                    new Product { Name = "T-Shirt", Description = "Casual wear", Price = 20, CategoryId = 3 },
                    new Product { Name = "Happy Numbers", Description = "Coloring book", Price = 4.5m, CategoryId = 2  }
                };

                await dbContext.Products.AddRangeAsync(products);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
