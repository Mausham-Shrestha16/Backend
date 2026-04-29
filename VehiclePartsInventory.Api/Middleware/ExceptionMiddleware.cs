using System.Net;
using System.Text.Json;

namespace VehiclePartsInventory.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = ex.Message
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}