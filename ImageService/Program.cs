using ImageService.Data;
using ImageService.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ImageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Fix: Specify the correct implementation class for IImageService
builder.Services.AddScoped<IImageService, ImageService.Service.ImageService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => // Ensure SwaggerGen is properly configured
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImageService API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // To serve wwwroot/images

app.UseAuthorization();
app.MapControllers();
app.Run();
