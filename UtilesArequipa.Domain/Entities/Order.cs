namespace UtilesArequipa.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? AddressId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; } = "pending";
    public decimal Subtotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
    public Address? Address { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<DeliveryRoute> DeliveryRoutes { get; set; } = new List<DeliveryRoute>();
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
}