using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkLogger.Api.Controllers.User;

[Route("[controller]")]
[Authorize]
public class GetMeController: ControllerBase
{
    [HttpGet]
    public IActionResult GetMe()
    {
        var name = User.FindFirstValue(ClaimTypes.Name);
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

        return Ok(new { name, roles });
    }
}
