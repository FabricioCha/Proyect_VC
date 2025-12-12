using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilesArequipa.Application.Interfaces.Persistence;

namespace UtilesArequipa.Application.Features.Kits.Queries.GetKits;

public class GetKitsQueryHandler : IRequestHandler<GetKitsQuery, List<KitDto>>
{
    private readonly IApplicationDbContext _context;

    public GetKitsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<KitDto>> Handle(GetKitsQuery request, CancellationToken cancellationToken)
    {
        var kits = await _context.Kits
            .Include(k => k.KitItems)
            .ThenInclude(ki => ki.Product)
            .Where(k => k.Active)
            .ToListAsync(cancellationToken);

        return kits.Select(k => new KitDto
        {
            Id = k.Id,
            Name = k.Name,
            Description = k.Description,
            Price = k.Price,
            VirtualStock = k.CalculateVirtualStock()
        }).ToList();
    }
}
