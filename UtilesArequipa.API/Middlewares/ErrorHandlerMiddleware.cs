using System.Net;
using System.Text.Json;

namespace UtilesArequipa.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            _logger.LogError(error, "Error no controlado en la solicitud");
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case ArgumentException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new 
            { 
                message = error?.Message, 
                innerException = error?.InnerException?.Message,
                detalle = _env.IsDevelopment() ? error?.StackTrace : "Error interno del servidor" 
            });
            await response.WriteAsync(result);
        }
    }
}
