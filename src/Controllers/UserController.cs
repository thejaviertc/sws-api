using Microsoft.AspNetCore.Mvc;
using SteamWorkshopStats.Models;
using SteamWorkshopStats.Services;

namespace SteamWorkshopStats.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private SteamService _steamService;

    public UserController(SteamService steamService)
    {
        _steamService = steamService;
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

        return new User
        {
            SteamId = steamId,
            Username = profileInfo.Username,
            ProfileImageUrl = profileInfo.ProfileImageUrl,
            Views = 0,
            Suscribers = 0,
            Favorites = 0,
            Likes = 0,
            Dislikes = 0,
            Addons = addons
        };
    }
}
