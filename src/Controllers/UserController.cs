using Microsoft.AspNetCore.Mvc;
using SteamWorkshopStats.Models;
using SteamWorkshopStats.Services;

namespace SteamWorkshopStats.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
	private readonly ISteamService _steamService;

	private readonly IDiscordService _discordService;

	public UserController(ISteamService steamService, IDiscordService discordService)
	{
		_steamService = steamService;
		_discordService = discordService;
	}

	/// <summary>
	/// Obtains an User given his ProfileID
	/// </summary>
	/// <param name="profileId">The ProfileID from the URL of the User's profile</param>
	/// <returns>An User</returns>
	[HttpGet("id/{profileId}")]
	public async Task<ActionResult<User>> GetUserByProfileId(string profileId)
	{
		_ = _discordService.LogQueryAsync(profileId);

		string? steamId = await _steamService.GetSteamIdAsync(profileId);

		if (steamId is null)
			return NotFound();

		return await GetUserAsync(steamId);
	}

	/// <summary>
	/// Obtains an User given his SteamID
	/// </summary>
	/// <param name="steamId">The SteamID of the User</param>
	/// <returns>An User</returns>
	[HttpGet("profiles/{steamId}")]
	public async Task<ActionResult<User>> GetUserBySteamId(string steamId)
	{
		_ = _discordService.LogQueryAsync(steamId);

		return await GetUserAsync(steamId);
	}

	/// <summary>
	/// Obtains an User given his SteamID
	/// </summary>
	/// <param name="steamId">The SteamID of the User</param>
	/// <returns>An User</returns>
	private async Task<ActionResult<User>> GetUserAsync(string steamId)
	{
		var profileInfo = await _steamService.GetProfileInfoAsync(steamId);

		if (profileInfo is null)
			return NotFound();

		var addons = await _steamService.GetAddonsAsync(steamId);

		var user = new User
		{
			SteamId = steamId,
			Username = profileInfo.Username,
			ProfileImageUrl = profileInfo.ProfileImageUrl,
			Views = addons.Sum(addon => addon.Views),
			Suscribers = addons.Sum(addon => addon.Suscribers),
			Favorites = addons.Sum(addon => addon.Favorites),
			Likes = addons.Sum(addon => addon.Likes),
			Dislikes = addons.Sum(addon => addon.Dislikes),
			Addons = addons
		};

		_ = _discordService.LogUserAsync(user);

		return user;
	}
}
