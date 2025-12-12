using MediatR;
using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.Application.Features.Kits.Queries.GetKits;

public class GetKitsQuery : IRequest<List<KitDto>>
{
}

public class KitDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int VirtualStock { get; set; }
}
