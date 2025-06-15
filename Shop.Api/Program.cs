using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();