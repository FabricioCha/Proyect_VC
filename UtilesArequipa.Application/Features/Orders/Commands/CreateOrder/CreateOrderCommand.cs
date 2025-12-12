using MediatR;

namespace UtilesArequipa.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<int>
{
    public int UserId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int? ProductId { get; set; }
    public int? KitId { get; set; }
    public int Quantity { get; set; }
}
