using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.Models;

namespace Shop.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ShopDbContext _context;

    public UserRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
}