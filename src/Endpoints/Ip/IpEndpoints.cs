using Microsoft.AspNetCore.Mvc;
using SaveIpApi.Authentication;
using SaveIpApi.Mappers;
using SaveIpApi.Repositories;

namespace SaveIpApi.Endpoints.Ip;

public static class IpEndpoints
{
    public static void RegisterIpEndpoints(this WebApplication app)
    {
        app.MapGet("ip/{key}/", async (IpAddressesRepository repository, string key) =>
        {
            var ip = await repository.GetLatest(key);
            if (ip is not null)
            {
                return Results.Ok(ip);
            }
            return Results.NotFound();
        })
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();

        app.MapPost("ip/{key}/", async (IpAddressesRepository repository, [FromBody] PostIpRequest model, string key) =>
        {
            await repository.Create(model.ToIpAdressEntity(key));
        })
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}

public record GetIpResponse(string Ip, string RecievedDate);

public record PostIpRequest(string Ip);