using MediatR;
using UtilesArequipa.Application.Features.Productos.DTOs;

namespace UtilesArequipa.Application.Features.Productos.Queries;

public class ObtenerProductosQuery : IRequest<IReadOnlyList<ProductoDto>>
{
}
