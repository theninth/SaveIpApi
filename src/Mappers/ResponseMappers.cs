using SaveIpApi.Endpoints.Ip;
using SaveIpApi.Models;

namespace SaveIpApi.Mappers;

public static class ResponseMappers
{
    public static GetIpResponse ToGetIpResponse(this IpAdressEntity entity)
        => new(entity.RecievedDate, entity.Key, entity.IpAddress);
}
