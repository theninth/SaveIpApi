
using Microsoft.Extensions.Options;
using SaveIpApi.Models.Options;

namespace SaveIpApi.Authentication;

public class ApiKeyAuthenticationEndpointFilter(IOptions<AppAuthenticationOptions> options) : IEndpointFilter
{
    private readonly AppAuthenticationOptions _options = options.Value;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string? providedApiKey = context.HttpContext.Request.Headers["ApiKey"];
        if (_options.ApiKey != providedApiKey)
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }
}
