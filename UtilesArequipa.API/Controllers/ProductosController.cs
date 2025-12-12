using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilesArequipa.Application.Features.Productos.Commands;
using UtilesArequipa.Application.Features.Productos.Queries;

namespace UtilesArequipa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Crear([FromBody] CrearProductoCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObtenerTodos), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var productos = await _mediator.Send(new ObtenerProductosQuery());
        return Ok(productos);
    }
}
