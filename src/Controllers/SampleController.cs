using Microsoft.AspNetCore.Mvc;

namespace SteamWorkshopStats.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    [HttpGet]
    public void Get()
    {
        throw new NotImplementedException();
    }
}
