using AutoMapper;
using MediatR;
using UtilesArequipa.Domain.Entities;
using UtilesArequipa.Domain.Interfaces;

namespace UtilesArequipa.Application.Features.Productos.Commands;

public class CrearProductoCommandHandler : IRequestHandler<CrearProductoCommand, int>
{
    private readonly IProductRepository _productoRepository;
    private readonly IMapper _mapper;

    public CrearProductoCommandHandler(IProductRepository productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CrearProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = _mapper.Map<Product>(request);
        var result = await _productoRepository.AddAsync(producto);
        return result.Id;
    }
}
