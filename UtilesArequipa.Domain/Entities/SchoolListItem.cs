namespace UtilesArequipa.Domain.Entities;

public class SchoolListItem
{
    public int Id { get; set; }
    public int? SchoolListId { get; set; }
    public int? ProductId { get; set; }
    public int SuggestedQuantity { get; set; }
    public string? Status { get; set; } = "available";

    public SchoolList? SchoolList { get; set; }
    public Product? Product { get; set; }
}