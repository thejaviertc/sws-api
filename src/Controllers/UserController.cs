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

    [HttpGet("id/{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        User user = new User
        {
            SteamId = await _steamService.LogTest(id),
            Username = "Paco",
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
