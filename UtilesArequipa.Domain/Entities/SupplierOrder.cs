namespace UtilesArequipa.Domain.Entities;

public class SupplierOrder
{
    public int Id { get; set; }
    public int? SupplierId { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal? Total { get; set; }
    public string? Status { get; set; } = "pending";

    public Supplier? Supplier { get; set; }
    public ICollection<SupplierOrderItem> SupplierOrderItems { get; set; } = new List<SupplierOrderItem>();
}