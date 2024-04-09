using SaveApi.DataAccess;
using Serilog;
using SaveIpApi.Repositories;
using SaveIpApi.Models.Options;
using SaveIpApi.Endpoints.Ip;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppAuthenticationOptions>(builder.Configuration.GetSection(AppAuthenticationOptions.SectionName));
builder.Services.AddTransient<DataContext>();
builder.Services.AddTransient<IpAddressesRepository>();

builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

app.UseHttpsRedirection();
app.RegisterIpEndpoints();

var context = app.Services.GetRequiredService<DataContext>();
await context.Init();

app.Run();
