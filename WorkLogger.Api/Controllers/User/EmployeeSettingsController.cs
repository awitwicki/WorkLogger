using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Domain.Entities;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers.User;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = ApplicationRoles.Admin)]
public class EmployeeSettingsController: ControllerBase
{
    private readonly IEmployeeSettingsService _employeeSettingsService;

    public EmployeeSettingsController(IEmployeeSettingsService employeeSettingsService)
    {
        _employeeSettingsService = employeeSettingsService;
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetEmployeeSettings()
    {
        // TODO
        var employeeId = Guid.NewGuid();
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }
        
        var result = await _employeeSettingsService.GetEmployeeSettings(employeeId);
        
        return Ok("null");
        return Ok(result);
    }
    
    [HttpPost]
    [Route("save")]
    public async Task<IActionResult> SaveOrUpdateEmployeeSettings(EmployeeSettings employeeSettings)
    {
        await _employeeSettingsService.SaveOrUpdateEmployeeSettings(employeeSettings);
        
        return Ok();
    }
}
