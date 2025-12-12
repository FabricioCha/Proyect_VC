namespace UtilesArequipa.Application.Features.Productos.DTOs;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string SKU { get; set; } = string.Empty;
}
