using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers.User;

[Route("[controller]")]
[Authorize]
public class GetMeController: ControllerBase
{
    
    [HttpGet]
    public IActionResult GetMe()
    {
        return Ok();
    }
}
