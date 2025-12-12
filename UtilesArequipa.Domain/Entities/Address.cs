namespace UtilesArequipa.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Street { get; set; } = null!;
    public string? District { get; set; }
    public string City { get; set; } = "Arequipa";
    public string? Reference { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}