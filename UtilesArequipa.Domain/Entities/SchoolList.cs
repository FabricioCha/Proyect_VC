namespace UtilesArequipa.Domain.Entities;

public class SchoolList
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? SchoolName { get; set; }
    public string? Grade { get; set; }
    public string? FileUrl { get; set; }
    public string? Status { get; set; } = "pending_review";
    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
    public ICollection<SchoolListItem> SchoolListItems { get; set; } = new List<SchoolListItem>();
}