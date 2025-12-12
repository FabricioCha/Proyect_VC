namespace UtilesArequipa.Domain.Entities;

public class DeliveryRoute
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public int? DriverId { get; set; }
    public string? District { get; set; }
    public string? DeliveryStatus { get; set; } = "assigned";
    public decimal? GpsLat { get; set; }
    public decimal? GpsLng { get; set; }
    public DateTime? DeliveredAt { get; set; }

    public Order? Order { get; set; }
    public User? Driver { get; set; }
}