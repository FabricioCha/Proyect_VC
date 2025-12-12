using AutoMapper;
using MediatR;
using UtilesArequipa.Application.Features.Productos.DTOs;
using UtilesArequipa.Domain.Interfaces;

namespace UtilesArequipa.Application.Features.Productos.Queries;

public class ObtenerProductosQueryHandler : IRequestHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>>
{
    private readonly IProductRepository _productoRepository;
    private readonly IMapper _mapper;

    public ObtenerProductosQueryHandler(IProductRepository productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductoDto>> Handle(ObtenerProductosQuery request, CancellationToken cancellationToken)
    {
        // En un escenario real, aquí podríamos aplicar paginación o filtros.
        // El repositorio genérico por defecto no implementa AsNoTracking directamente en GetAllAsync
        // pero para esta implementación base asumiremos que GetAllAsync es suficiente.
        // Si quisiéramos AsNoTracking explícito, deberíamos agregar un método al repositorio o usar una especificación.
        
        var productos = await _productoRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<ProductoDto>>(productos);
    }
}
