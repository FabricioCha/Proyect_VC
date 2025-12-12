namespace UtilesArequipa.Domain.Entities;

public class KitItem
{
    public int Id { get; set; }
    public int? KitId { get; set; }
    public int? ProductId { get; set; }
    public int Quantity { get; set; }

    public Kit? Kit { get; set; }
    public Product? Product { get; set; }
}