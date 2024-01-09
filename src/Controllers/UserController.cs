using Microsoft.AspNetCore.Mvc;
using SteamWorkshopStats.Models;
using SteamWorkshopStats.Services;

namespace SteamWorkshopStats.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private DiscordService _discordService;

    public UserController(DiscordService discordService)
    {
        _discordService = discordService;
    }

    [HttpGet("id/{id}")]
    public ActionResult<User> GetUserById(string id)
    {
        User user = new User
        {
            SteamId = id,
            Username = _discordService.Test(),
            ProfileImageUrl = "adadas",
            Views = 10,
            Suscribers = 100,
            Favorites = 12,
            Likes = 10,
            Dislikes = 2,
            Addons = new List<Addon>()
        };

        return user;
    }

    [HttpGet("profiles/{steamId}")]
    public IActionResult GetUserBySteamId(string steamId)
    {
        throw new NotImplementedException();
    }
}
