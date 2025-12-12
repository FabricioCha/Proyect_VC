using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilesArequipa.Application.Interfaces.Persistence;
using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var order = new Order
            {
                UserId = request.UserId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                Total = 0
            };

            decimal totalAmount = 0;

            foreach (var itemDto in request.Items)
            {
                if (itemDto.ProductId.HasValue)
                {
                    var product = await _context.Products.FindAsync(new object[] { itemDto.ProductId.Value }, cancellationToken);
                    if (product == null) throw new KeyNotFoundException($"Producto {itemDto.ProductId} no encontrado.");

                    if (product.Stock < itemDto.Quantity)
                    {
                        throw new ValidationException($"Stock insuficiente para el producto {product.Name}. Stock actual: {product.Stock}");
                    }

                    product.Stock -= itemDto.Quantity;
                    // El RowVersion se verificará automáticamente por EF Core al guardar cambios

                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = itemDto.Quantity,
                        UnitPrice = product.Price
                    };
                    order.OrderItems.Add(orderItem);
                    totalAmount += orderItem.Quantity * orderItem.UnitPrice;
                }
                else if (itemDto.KitId.HasValue)
                {
                    var kit = await _context.Kits
                        .Include(k => k.KitItems)
                        .ThenInclude(ki => ki.Product)
                        .FirstOrDefaultAsync(k => k.Id == itemDto.KitId.Value, cancellationToken);

                    if (kit == null) throw new KeyNotFoundException($"Kit {itemDto.KitId} no encontrado.");

                    foreach (var kitItem in kit.KitItems)
                    {
                        var requiredQuantity = kitItem.Quantity * itemDto.Quantity;
                        var product = kitItem.Product;

                        if (product == null) continue;

                        if (product.Stock < requiredQuantity)
                        {
                            throw new ValidationException($"Stock insuficiente para el componente {product.Name} del kit {kit.Name}. Requerido: {requiredQuantity}, Disponible: {product.Stock}");
                        }

                        product.Stock -= requiredQuantity;
                    }

                    var orderItem = new OrderItem
                    {
                        KitId = kit.Id,
                        Quantity = itemDto.Quantity,
                        UnitPrice = kit.Price
                    };
                    order.OrderItems.Add(orderItem);
                    totalAmount += orderItem.Quantity * orderItem.UnitPrice;
                }
            }

            order.Total = totalAmount;
            _context.Orders.Add(order);
            
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return order.Id;
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new Exception("El stock cambió mientras procesabas la compra, intenta de nuevo.");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
