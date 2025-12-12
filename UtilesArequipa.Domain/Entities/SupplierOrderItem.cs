namespace UtilesArequipa.Domain.Entities;

public class SupplierOrderItem
{
    public int Id { get; set; }
    public int? SupplierOrderId { get; set; }
    public int? ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public decimal Subtotal { get; set; }

    public SupplierOrder? SupplierOrder { get; set; }
    public Product? Product { get; set; }
}