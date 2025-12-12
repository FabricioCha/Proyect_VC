namespace UtilesArequipa.Domain.Entities;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<SupplierOrder> SupplierOrders { get; set; } = new List<SupplierOrder>();
}