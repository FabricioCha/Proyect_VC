namespace UtilesArequipa.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public int? ParentId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Category? Parent { get; set; }
    public ICollection<Category> InverseParent { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}