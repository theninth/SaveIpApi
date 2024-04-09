using SaveIpApi.Endpoints.Ip;
using SaveIpApi.Models;

namespace SaveIpApi.Mappers;

public static class RequestMappers
{
    public static IpAdressEntity ToIpAdressEntity(this PostIpRequest ipRequest, string key) =>  new(DateTime.Now, key, ipRequest.Ip);
}
