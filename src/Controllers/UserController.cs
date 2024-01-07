using Microsoft.AspNetCore.Mvc;
using SteamWorkshopStats.Models;

namespace SteamWorkshopStats.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet("id/{id}")]
    public ActionResult<User> GetUserById(string id)
    {
        User user = new User
        {
            SteamId = id,
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
