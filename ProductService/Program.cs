using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductService.Data;
using ProductService.Services; // Ensure this namespace contains the correct ProductService class

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Fix: Fully qualify the ProductService class or ensure the correct namespace is used
builder.Services.AddScoped<IProductService, ProductService.Services.ProductService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductService API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService API V1");
    });
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();

app.Run();
