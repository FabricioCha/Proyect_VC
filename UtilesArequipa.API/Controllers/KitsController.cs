using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilesArequipa.Application.Features.Kits.Commands;
using UtilesArequipa.Infrastructure.Services;

namespace UtilesArequipa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KitsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public KitsController(IMediator mediator, IBackgroundJobClient backgroundJobClient)
    {
        _mediator = mediator;
        _backgroundJobClient = backgroundJobClient;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Crear([FromBody] CrearKitCommand command)
    {
        // 1. Crear el Kit (Transaccional)
        var kitId = await _mediator.Send(command);

        // 2. Encolar tarea en segundo plano (Fire-and-forget)
        // Solo si la creación fue exitosa.
        try
        {
            _backgroundJobClient.Enqueue<NotificacionService>(service => 
                service.EnviarNotificacionNuevoKit(command.Nombre));
        }
        catch (Exception ex)
        {
            // Loguear el error pero no fallar la solicitud
            // Aquí deberíamos usar un ILogger, pero por brevedad solo mostramos la intención
            Console.WriteLine($"Error al encolar notificación: {ex.Message}");
        }

        return CreatedAtAction(nameof(Crear), new { id = kitId }, new { id = kitId });
    }
}
