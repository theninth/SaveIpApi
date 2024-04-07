using SaveIpApi.Models;

namespace SaveIpApi.Mappers;

public static class RequestMappers
{
    public static IpAdressEntity ToIpAdressEntity(this IpRequest ipRequest, string key) =>  new(DateTime.Now, key, ipRequest.Ip);
}
