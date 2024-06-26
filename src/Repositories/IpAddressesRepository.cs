﻿using Dapper;
using SaveApi.DataAccess;
using SaveIpApi.Endpoints.Ip;
using SaveIpApi.Models;

namespace SaveIpApi.Repositories;

public class IpAddressesRepository(ILogger<IpAddressesRepository> logger, DataContext context)
{
    private readonly ILogger<IpAddressesRepository> _logger = logger;
    private readonly DataContext _context = context;

    public async Task<IpAdressEntity?> GetLatest(string key)
    {
        using var connection = _context.CreateConnection();
        _logger.LogInformation("Recieved request for '{key}'.", key);
        var sql = "SELECT RecievedDate, Key, IpAddress AS Ip FROM IpAddresses WHERE Key = @Key ORDER BY RecievedDate DESC LIMIT 1";
        var value = await connection.QueryFirstOrDefaultAsync<dynamic> (sql, new { Key = key });
        if (value is not null)
        {
            DateTime recievedDate = DateTime.MinValue;
            if (DateTime.TryParse(value.RecievedDate, out DateTime dt))
            {
                recievedDate = dt;
            };

            return new IpAdressEntity(recievedDate, value.Key, value.Ip);
        }
        return null;
    }

    public async Task Create(IpAdressEntity ipAdress)
    {
        using var connection = _context.CreateConnection();

        var sql = "SELECT IpAddress AS Ip FROM IpAddresses WHERE Key = @Key ORDER BY RecievedDate DESC LIMIT 1";
        var latestIp = await connection.QueryFirstOrDefaultAsync<string>(sql, new { ipAdress.Key });

        if (latestIp != ipAdress.IpAddress)
        {
            _logger.LogInformation("Recieved ip address '{ipAddress}' for key '{key}'.", ipAdress.IpAddress, ipAdress.Key);
            sql = "INSERT INTO IpAddresses (RecievedDate, Key, IpAddress) VALUES (@RecievedDate, @Key, @IpAddress);";
            await connection.ExecuteAsync(sql, ipAdress);
        }
        else
        {
            _logger.LogInformation("Ignoring ip address '{ipAddress}' for '{key}' (already stored in database).", ipAdress.IpAddress, ipAdress.Key);
        }
    }
}
