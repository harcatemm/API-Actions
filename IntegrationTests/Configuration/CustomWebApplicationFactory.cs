using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Infraestructure.Persistence; 
using Domain.Entities;

namespace IntegrationTests.Configuration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Quitar la configuración real del DbContext si existe
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                // Añadir DbContext de pruebas (InMemory) con DB única por instancia
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });


                // Crear BD en memoria
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();                    
                    
                    // Semilla de prueba (puedes personalizar)
                    if (!db.Categories.Any() && !db.Products.Any())
                    {
                        Task.WaitAll(DataSeed.SeedAsync(db));
                    }

                    if(!db.Users.Any())
                        Task.WaitAll(IdentitySeed.SeedAsync(scope.ServiceProvider));                    
                }
            });
        }
    }
}
