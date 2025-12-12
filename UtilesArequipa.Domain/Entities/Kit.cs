namespace UtilesArequipa.Domain.Entities;

public class Kit
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Grade { get; set; }
    public string? SchoolName { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<KitItem> KitItems { get; set; } = new List<KitItem>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public int CalculateVirtualStock()
    {
        if (!KitItems.Any()) return 0;
        return KitItems.Min(item => item.Product != null && item.Quantity > 0 
            ? item.Product.Stock / item.Quantity 
            : 0);
    }
}