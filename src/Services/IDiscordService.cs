using SteamWorkshopStats.Models;

namespace SteamWorkshopStats.Services;

public interface IDiscordService
{
	Task LogQueryAsync(string value);

	Task LogUserAsync(User user);
}
