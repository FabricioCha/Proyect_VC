namespace UtilesArequipa.Domain.Entities;

public class InventoryLog
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public string? Type { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public Product? Product { get; set; }
}