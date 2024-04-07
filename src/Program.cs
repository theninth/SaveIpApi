using SaveApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SaveIpApi.Repositories;
using SaveIpApi.Mappers;
using System.IO;

// https://jasonwatmore.com/net-7-dapper-sqlite-crud-api-tutorial-in-aspnet-core

var builder = WebApplication.CreateBuilder(args);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("updateip/{key}/", async (IpAddressesRepository repository, [FromBody]IpRequest model, string key) =>
{
    await repository.Create(model.ToIpAdressEntity(key));
    Console.WriteLine($"{key}: {model.Ip}");
})
.WithName("UpdateIp")
.WithOpenApi();

var context = app.Services.GetRequiredService<DataContext>();
await context.Init();

app.Run();

public record IpRequest(string Ip);