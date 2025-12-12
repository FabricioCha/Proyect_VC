using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
