using Microsoft.EntityFrameworkCore;
using UtilesArequipa.Domain.Entities;
using UtilesArequipa.Domain.Interfaces;
using UtilesArequipa.Infrastructure.Persistence.Contexts;

namespace UtilesArequipa.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
    }
}
