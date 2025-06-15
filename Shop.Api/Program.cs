using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using Shop.Infrastructure;
using Shop.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ShopDbContext>(options =>
    // NOTE: For simplicity in this demo, the connection string is hardcoded.
    // In a real-world application I would use appsettings.json and access it as a configuration
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=user;Password=p"));

builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<ICartRepository, CartRepository>();

builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
    
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();