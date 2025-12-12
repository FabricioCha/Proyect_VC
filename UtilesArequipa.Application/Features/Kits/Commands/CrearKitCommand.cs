using MediatR;
using UtilesArequipa.Application.Features.Kits.DTOs;

namespace UtilesArequipa.Application.Features.Kits.Commands;

public class CrearKitCommand : IRequest<int>
{
    public string Nombre { get; set; } = string.Empty;
    public List<KitItemDto> Items { get; set; } = new();
}
