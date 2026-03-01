using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Domain.Entities;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers.User;

public class EmployeeSettingsDto
{
    public string FullName { get; set; }
    public DateTimeOffset ContractStartedDate { get; set; }
    public int VacationDaysPerYear { get; set; }
}

[ApiController]
[Route("[controller]")]
[Authorize(Roles = $"{ApplicationRoles.Admin},{ApplicationRoles.UserRole}")]
public class EmployeeSettingsController: ControllerBase
{
    private readonly IEmployeeSettingsService _employeeSettingsService;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeSettingsController(IEmployeeSettingsService employeeSettingsService, UserManager<ApplicationUser> userManager)
    {
        _employeeSettingsService = employeeSettingsService;
        _userManager = userManager;
    }

    private async Task<Guid?> GetCurrentUserIdAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return null;
        var user = await _userManager.FindByEmailAsync(email);
        return user?.Id;
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetEmployeeSettings()
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return Unauthorized();

        var result = await _employeeSettingsService.GetEmployeeSettings(userId.Value);
        return Ok(result);
    }

    [Authorize(Roles = ApplicationRoles.Admin)]
    [HttpGet]
    [Route("get/{userId}")]
    public async Task<IActionResult> GetEmployeeSettingsById(Guid userId)
    {
        var result = await _employeeSettingsService.GetEmployeeSettings(userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("save")]
    public async Task<IActionResult> SaveOrUpdateEmployeeSettings(EmployeeSettingsDto dto)
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return Unauthorized();

        var employeeSettings = new EmployeeSettings
        {
            EmployeeId = userId.Value,
            FullName = dto.FullName,
            ContractStartedDate = dto.ContractStartedDate,
            VacationDaysPerYear = dto.VacationDaysPerYear
        };

        await _employeeSettingsService.SaveOrUpdateEmployeeSettings(employeeSettings);
        return Ok();
    }

    [Authorize(Roles = ApplicationRoles.Admin)]
    [HttpPost]
    [Route("save/{userId}")]
    public async Task<IActionResult> SaveOrUpdateEmployeeSettingsById(Guid userId, EmployeeSettingsDto dto)
    {
        var employeeSettings = new EmployeeSettings
        {
            EmployeeId = userId,
            FullName = dto.FullName,
            ContractStartedDate = dto.ContractStartedDate,
            VacationDaysPerYear = dto.VacationDaysPerYear
        };

        await _employeeSettingsService.SaveOrUpdateEmployeeSettings(employeeSettings);
        return Ok();
    }
}
