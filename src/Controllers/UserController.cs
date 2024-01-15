using Microsoft.AspNetCore.Mvc;
using SteamWorkshopStats.Models;
using SteamWorkshopStats.Services;

namespace SteamWorkshopStats.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private ISteamService _steamService;
    private DiscordService _discordService;

    public UserController(ISteamService steamService, DiscordService discordService)
    {
        _steamService = steamService;
        _discordService = discordService;
    }

    [HttpGet("id/{profileId}")]
    public async Task<ActionResult<User>> GetUserByProfileId(string profileId)
    {
        string steamId = await _steamService.GetSteamId(profileId);

        if (steamId is null)
            return NotFound();

        return await GetUser(steamId);
    }

    [HttpGet("profiles/{steamId}")]
    public async Task<ActionResult<User>> GetUserBySteamId(string steamId)
    {
        return await GetUser(steamId);
    }

    private async Task<ActionResult<User>> GetUser(string steamId)
    {
        var profileInfo = await _steamService.GetProfileInfo(steamId);

        if (profileInfo is null)
            return NotFound();

        var addons = await _steamService.GetAddons(steamId);

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

        _discordService.LogUser(user);

        return user;
    }
}
