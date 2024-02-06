using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SteamWorkshopStats.Models;
using SteamWorkshopStats.Services;

namespace SteamWorkshopStats.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting("fixed")]
public class UserController : ControllerBase
{
	private readonly IWebHostEnvironment _env;

	private readonly ISteamService _steamService;

	private readonly IDiscordService _discordService;

	public UserController(IWebHostEnvironment env, ISteamService steamService, IDiscordService discordService)
	{
		_env = env;
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
		string? steamId = await _steamService.GetSteamIdAsync(profileId);

		if (steamId is null)
			return NotFound(new { Message = "This User doesn't exists (SteamID not found)" });

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
			return NotFound(new { Message = "This User doesn't exists (Profile info not found)" });

		var addons = await _steamService.GetAddonsAsync(steamId);

		var user = new User
		{
			SteamId = steamId,
			Username = profileInfo.Username,
			ProfileImageUrl = profileInfo.ProfileImageUrl,
			Views = addons.Sum(addon => addon.Views),
			Subscribers = addons.Sum(addon => addon.Subscribers),
			Favorites = addons.Sum(addon => addon.Favorites),
			Likes = addons.Sum(addon => addon.Likes),
			Dislikes = addons.Sum(addon => addon.Dislikes),
			Addons = addons
		};

		if (_env.IsProduction())
		{
			_ = _discordService.LogUserAsync(user);
		}

		return user;
	}
}
