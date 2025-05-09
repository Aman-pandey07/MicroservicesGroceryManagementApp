﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore; // Add this using directive for 'UseSqlServer'
using Microsoft.IdentityModel.Tokens;
using OrderService.Data;
using OrderService.RabbitMQ.Services;
using OrderService.Services;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//authorization added
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = builder.Configuration["Jwt:Issuer"],
             ValidAudience = builder.Configuration["Jwt:Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
             ),
             RoleClaimType = ClaimTypes.Role
         };
     });

builder.Services.AddAuthorization();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Ensure 'Microsoft.EntityFrameworkCore.SqlServer' package is installed


builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
// Ensure the NuGet package 'Microsoft.EntityFrameworkCore.SqlServer' is installed in your project.
// You can install it using the following command in the Package Manager Console:
// Install-Package Microsoft.EntityFrameworkCore.SqlServer
// Add HttpClient
builder.Services.AddHttpClient("ProductService", c =>
{
    c.BaseAddress = new Uri("https://localhost:7087/api/Product/"); // Replace with ProductService actual base URL
});

// Add Services
builder.Services.AddScoped<IOrderService, OrderService.Services.OrderService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
    {
        Console.WriteLine("🧾 Authenticated User:");
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"👉 {claim.Type} = {claim.Value}");
        }
    }
    else
    {
        Console.WriteLine("❌ User NOT authenticated");
    }

    await next();
});


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.Run();

