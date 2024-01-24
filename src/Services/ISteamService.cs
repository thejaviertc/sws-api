using SteamWorkshopStats.Models;
using SteamWorkshopStats.Models.Records;

namespace SteamWorkshopStats.Services;

public interface ISteamService
{
	Task<List<Addon>> GetAddons(string steamId);

	Task<GetPlayerSummariesPlayer> GetProfileInfo(string steamId);

	Task<string> GetSteamId(string profileId);
}
