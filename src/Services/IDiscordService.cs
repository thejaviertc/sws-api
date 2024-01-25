using SteamWorkshopStats.Models;

namespace SteamWorkshopStats.Services;

public interface IDiscordService
{
	Task LogQuery(string value);

	Task LogUser(User user);
}
