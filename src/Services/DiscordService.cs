namespace SteamWorkshopStats.Services;

public class DiscordService
{
    private readonly IConfiguration _configuration;

    public DiscordService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Test() => _configuration["Test"];
}
