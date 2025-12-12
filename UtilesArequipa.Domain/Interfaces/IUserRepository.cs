using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
