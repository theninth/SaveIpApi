namespace SaveApi.DataAccess;

using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

public class DataContext(IConfiguration config)
{
    public IDbConnection CreateConnection()
        => new SqliteConnection(config.GetConnectionString("Default"));

    public async Task Init()
    {
        using var connection = CreateConnection();
        var sql = """
                CREATE TABLE IF NOT EXISTS 
                IpAddresses (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Key TEXT,
                    RecievedDate TEXT,
                    IpAddress TEXT
                );
            """;
        await connection.ExecuteAsync(sql);
    }
}