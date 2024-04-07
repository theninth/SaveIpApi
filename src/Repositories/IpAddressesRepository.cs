using Dapper;
using SaveApi.Helpers;
using SaveIpApi.Models;

namespace SaveIpApi.Repositories;

public class IpAddressesRepository(DataContext context)
{
    private readonly DataContext _context = context;

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
