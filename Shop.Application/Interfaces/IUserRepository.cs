using Shop.Application.Models;

namespace Shop.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
}