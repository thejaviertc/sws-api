using SteamWorkshopStats.Models;
using SteamWorkshopStats.Models.Records;

namespace SteamWorkshopStats.Services;

public interface ISteamService
{
	Task<List<Addon>> GetAddonsAsync(string steamId);

	Task<GetPlayerSummariesPlayer?> GetProfileInfoAsync(string steamId);

	Task<string?> GetSteamIdAsync(string profileId);
}
