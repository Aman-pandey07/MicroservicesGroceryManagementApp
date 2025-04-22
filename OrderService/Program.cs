using Microsoft.EntityFrameworkCore; // Add this using directive for 'UseSqlServer'
using OrderService.Data;
using OrderService.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Ensure 'Microsoft.EntityFrameworkCore.SqlServer' package is installed


// Ensure the NuGet package 'Microsoft.EntityFrameworkCore.SqlServer' is installed in your project.
// You can install it using the following command in the Package Manager Console:
// Install-Package Microsoft.EntityFrameworkCore.SqlServer
// Add HttpClient
builder.Services.AddHttpClient("ProductService", c =>
{
    c.BaseAddress = new Uri("https://localhost:7087"); // Replace with ProductService actual base URL
});

// Add Services
builder.Services.AddScoped<IOrderService, OrderService.Services.OrderService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.UseAuthorization();
app.Run();

