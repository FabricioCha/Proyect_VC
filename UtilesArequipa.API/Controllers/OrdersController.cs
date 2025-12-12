using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilesArequipa.Application.Features.Orders.Commands.CreateOrder;

namespace UtilesArequipa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim == null)
        {
            return Unauthorized("Usuario no identificado en el token.");
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return BadRequest("ID de usuario inválido en el token.");
        }

        var command = new CreateOrderCommand
        {
            UserId = userId,
            Items = request.Items
        };
        
        var orderId = await _mediator.Send(command);
        return Ok(new { OrderId = orderId });
    }
}

public record CreateOrderRequest(List<OrderItemDto> Items);