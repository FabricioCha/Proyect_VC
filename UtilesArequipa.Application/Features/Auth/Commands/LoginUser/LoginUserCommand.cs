using MediatR;

namespace UtilesArequipa.Application.Features.Auth.Commands.LoginUser;

public class LoginUserCommand : IRequest<string>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
