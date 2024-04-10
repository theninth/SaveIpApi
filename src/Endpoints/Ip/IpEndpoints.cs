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
            var ipEntity = await repository.GetLatest(key);
            if (ipEntity is not null)
            {
                return Results.Ok(ipEntity.ToGetIpResponse());
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

public record GetIpResponse(DateTime RecievedDate, string Key, string IpAddress);

public record PostIpRequest(string Ip);