using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = ApplicationRoles.Admin)]
public class MonthDayController: ControllerBase
{
    private readonly IMonthDayService _monthDayService;

    public MonthDayController(IMonthDayService monthDayService)
    {
        _monthDayService = monthDayService;
    }
    
    [Authorize(Roles = ApplicationRoles.UserRole)]
    [HttpGet]
    public async Task<IActionResult> BuildMonth(DateTimeOffset date)
    {
        var result = await _monthDayService.BuildMonth(date);
        return Ok(result);
    }
    
    [Authorize(Roles = ApplicationRoles.Admin)]
    [HttpGet]
    [Route("GetDaysInMonth")]
    public async Task<IActionResult> GetDaysInMonth(DateTimeOffset date, Guid userId)
    {
        var result = await _monthDayService.GetDaysInMonth(date, userId);
        return Ok(result);
    }
    
    [Authorize(Roles = ApplicationRoles.Admin)]
    [HttpGet]
    [Route("GetMonth")]
    public async Task<IActionResult> GetMonth(DateTimeOffset date, Guid userId)
    {
        var result = await _monthDayService.GetMonth(date, userId);
        return Ok(result);
    }
    
    [Authorize(Roles = ApplicationRoles.Admin)]
    [HttpGet]
    [Route("GetUserMonths")]
    public async Task<IActionResult> GetUserMonths(Guid userId)
    {
        var result = await _monthDayService.GetUserMonths(userId);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("SaveMonth")]
    public async Task<IActionResult> SaveMonth(IEnumerable<WorkDayViewModel> days, DateTimeOffset date, Guid userId)
    {
        await _monthDayService.SaveMonth(days, date, userId);
        return Ok();
    }
}
