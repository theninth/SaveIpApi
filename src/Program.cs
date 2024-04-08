using SaveApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SaveIpApi.Repositories;
using SaveIpApi.Mappers;
using SaveIpApi.Models.Options;
using SaveIpApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppAuthenticationOptions>(builder.Configuration.GetSection(AppAuthenticationOptions.SectionName));
builder.Services.AddTransient<DataContext>();
builder.Services.AddTransient<IpAddressesRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("ip/{key}/", async (IpAddressesRepository repository, string key) =>
{
    var ip = await repository.GetLatest(key);
    if (ip is not null)
    {
        return Results.Ok(ip);
    }
    return Results.NotFound();
})
.WithName("GetLatestIp")
.WithOpenApi()
.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();

app.MapPost("ip/{key}/", async (IpAddressesRepository repository, [FromBody]IpRequest model, string key) =>
{
    await repository.Create(model.ToIpAdressEntity(key));
})
.WithName("UpdateIp")
.WithOpenApi()
.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();

var context = app.Services.GetRequiredService<DataContext>();
await context.Init();

app.Run();

public record IpRequest(string Ip);

public record IpResponse(string Ip, string RecievedDate);