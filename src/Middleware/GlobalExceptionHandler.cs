using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Text.Json;

namespace SaveIpApi.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception ex,
        CancellationToken cancellationToken)
    {
        var traceId = Guid.NewGuid().ToString();
        _logger.LogError(ex, "Unhandled exception. traceId: {traceId}", traceId);
        const string contentType = "application/problem+json";
        var json = JsonSerializer.Serialize(new
        {
            Error = new
            {
                Code = 500,
                Message = $"An error occured. traceId = {traceId}."
            }
        });

        context.Response.ContentType = contentType;
        await context.Response.WriteAsync(json, cancellationToken);
        return true;
    }
}
