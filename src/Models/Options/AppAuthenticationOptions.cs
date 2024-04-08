namespace SaveIpApi.Models.Options;

public class AppAuthenticationOptions
{
    public const string SectionName = "Authentication";
    public string? ApiKey { get; set; }
}
