namespace UtilesArequipa.Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }

    public Product? Product { get; set; }
}