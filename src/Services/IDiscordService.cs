using SteamWorkshopStats.Models;

namespace SteamWorkshopStats.Services;

public interface IDiscordService
{
	Task LogQueryAsync(string path, string ip);

	Task LogUserAsync(User user);

	Task LogErrorAsync(string path, string ip, string message);
}
