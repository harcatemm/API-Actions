using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infraestructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Caso 1: recibir connection string por parámetro (args[0])
            var connectionString = args.Length > 0
                ? args[0]
                : "Server=(localdb)\\MSSQLLocaldb;Database=CatalogDB;User Id=sqlmcp;Password=SQL12345;MultipleActiveResultSets=true;"; // fallback

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
