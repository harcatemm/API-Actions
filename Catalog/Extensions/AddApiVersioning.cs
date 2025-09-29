using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Catalog.Extensions
{
    public static class AddApiVersioning
    {
        public static IServiceCollection AddCustomVersioning(this IServiceCollection services)
        {
            // Versionamiento
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }
    }
}
