using MediatR;
using UtilesArequipa.Domain.Entities;
using UtilesArequipa.Domain.Interfaces;

namespace UtilesArequipa.Application.Features.Kits.Commands;

public class CrearKitCommandHandler : IRequestHandler<CrearKitCommand, int>
{
    private readonly IGenericRepository<Kit> _kitRepository;
    private readonly IGenericRepository<Product> _productoRepository;

    public CrearKitCommandHandler(
        IGenericRepository<Kit> kitRepository, 
        IGenericRepository<Product> productoRepository)
    {
        _kitRepository = kitRepository;
        _productoRepository = productoRepository;
    }

    public async Task<int> Handle(CrearKitCommand request, CancellationToken cancellationToken)
    {
        var kit = new Kit
        {
            Name = request.Nombre
        };

        // Validar y agregar items
        foreach (var itemDto in request.Items)
        {
            var producto = await _productoRepository.GetByIdAsync(itemDto.ProductoId);
            if (producto == null)
            {
                throw new KeyNotFoundException($"El producto con ID {itemDto.ProductoId} no existe.");
            }

            var kitItem = new KitItem
            {
                ProductId = itemDto.ProductoId,
                Quantity = itemDto.Cantidad,
                Product = producto
            };
            kit.KitItems.Add(kitItem);
        }

        // Calcular precio total basado en los productos asignados
        kit.Price = kit.KitItems.Sum(i => i.Quantity * (i.Product?.Price ?? 0));

        // Guardar en base de datos
        // EF Core manejará la transacción implícita al llamar a SaveChanges dentro del repositorio
        await _kitRepository.AddAsync(kit);

        return kit.Id;
    }
}
