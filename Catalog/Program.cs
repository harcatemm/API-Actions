using Catalog.Entity;
using Catalog.Extensions;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<Config>(builder.Configuration);
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddCustomVersioning();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var provider = scope.ServiceProvider;
    var db = provider.GetRequiredService<AppDbContext>();

    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
        await IdentitySeed.SeedAsync(provider);
        await DataSeed.SeedAsync(db);
    }
    else
        db.Database.EnsureCreated();    
}


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            $"My API {description.ApiVersion}");
    }

    // Opcional: que arranque con la primera versión
    options.RoutePrefix = "swagger";        
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }