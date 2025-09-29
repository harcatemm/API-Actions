using APP.Interfaces;
using APP.Services;
using Catalog.Entity;
using Infraestructure.Identity;
using Infraestructure.Persistence;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Catalog.Extensions
{
    public static class AddService
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var cfg = sp.GetRequiredService<IOptions<Config>>().Value;
                options.UseSqlServer(cfg.ConnectionStrings.DefaultConnection);
            });

            services.AddSingleton<Config>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
