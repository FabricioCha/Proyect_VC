using MediatR;

namespace UtilesArequipa.Application.Features.Productos.Commands;

public class CrearProductoCommand : IRequest<int>
{
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
}
