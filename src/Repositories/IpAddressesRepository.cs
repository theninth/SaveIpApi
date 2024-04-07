using Dapper;
using SaveApi.Helpers;
using SaveIpApi.Models;

namespace SaveIpApi.Repositories;

public class IpAddressesRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<IpResponse?> GetLatest(string key)
    {
        using var connection = _context.CreateConnection();
        var sql =
            """
                SELECT IpAddress AS Ip, RecievedDate FROM IpAddresses WHERE Key = @Key ORDER BY RecievedDate DESC LIMIT 1
            """;
        return await connection.QueryFirstOrDefaultAsync<IpResponse>(sql, new { Key = key });
    }

    public async Task Create(IpAdressEntity ipAdress)
    {
        using var connection = _context.CreateConnection();
        var sql =
            """
                INSERT INTO IpAddresses (RecievedDate, Key, IpAddress) VALUES (@RecievedDate, @Key, @IpAddress);
            """;
        await connection.ExecuteAsync(sql, ipAdress);
    }
}
