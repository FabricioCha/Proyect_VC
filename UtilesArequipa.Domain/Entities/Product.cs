using System.ComponentModel.DataAnnotations;

namespace UtilesArequipa.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Sku { get; set; } = null!;
    public int? CategoryId { get; set; }
    public string? Brand { get; set; }
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }
    public int Stock { get; set; }
    public int StockMin { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ConcurrencyCheck]
    public uint RowVersion { get; set; }

    public Category? Category { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public ICollection<KitItem> KitItems { get; set; } = new List<KitItem>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();
    public ICollection<SupplierOrderItem> SupplierOrderItems { get; set; } = new List<SupplierOrderItem>();
    public ICollection<SchoolListItem> SchoolListItems { get; set; } = new List<SchoolListItem>();
}