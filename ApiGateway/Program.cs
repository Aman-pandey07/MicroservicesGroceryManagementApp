using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Token Validator dependency added
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();




app.MapReverseProxy();
app.MapControllers();

////this is temp log 
//app.Use(async (context, next) =>
//{
//    Console.WriteLine("➡️ Incoming Request: " + context.Request.Path);
//    Console.WriteLine("🔐 Authorization Header: " + context.Request.Headers["Authorization"]);
//    await next();
//});

app.Run();
