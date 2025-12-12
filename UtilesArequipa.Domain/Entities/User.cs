namespace UtilesArequipa.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "customer";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    public ICollection<DeliveryRoute> DeliveryRoutes { get; set; } = new List<DeliveryRoute>();
    public ICollection<SchoolList> SchoolLists { get; set; } = new List<SchoolList>();
}
