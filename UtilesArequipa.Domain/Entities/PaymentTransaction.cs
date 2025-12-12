namespace UtilesArequipa.Domain.Entities;

public class PaymentTransaction
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public string? Provider { get; set; }
    public string? TransactionId { get; set; }
    public decimal? Amount { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Order? Order { get; set; }
}