using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = $"{ApplicationRoles.Admin},{ApplicationRoles.UserRole}")]
public class MonthDayController: ControllerBase
{
    private readonly IMonthDayService _monthDayService;
    private readonly IPdfService _pdfService;
    private readonly UserManager<ApplicationUser> _userManager;

    public MonthDayController(IMonthDayService monthDayService, IPdfService pdfService, UserManager<ApplicationUser> userManager)
    {
        _monthDayService = monthDayService;
        _pdfService = pdfService;
        _userManager = userManager;
    }

    private async Task<Guid?> GetCurrentUserIdAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return null;
        var user = await _userManager.FindByEmailAsync(email);
        return user?.Id;
    }

    // ── User endpoints ──

    [HttpGet]
    [Route("MyMonth")]
    public async Task<IActionResult> MyMonth(DateTimeOffset date)
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return Unauthorized();

        // Try to get saved data first
        var saved = await _monthDayService.GetDaysInMonth(date, userId.Value);
        if (saved != null) return Ok(saved);

        // Fall back to building a template
        var template = await _monthDayService.BuildMonth(date);
        return Ok(template);
    }

    [HttpPost]
    [Route("SaveMyMonth")]
    public async Task<IActionResult> SaveMyMonth(IEnumerable<WorkDayViewModel> days, DateTimeOffset date)
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return Unauthorized();

        await _monthDayService.SaveMonth(days, date, userId.Value);
        return Ok();
    }

    [HttpGet]
    [Route("MyMonths")]
    public async Task<IActionResult> MyMonths()
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return Unauthorized();

        var result = await _monthDayService.GetUserMonths(userId.Value);
        return Ok(result);
    }

    [HttpGet]
    [Route("ExportMyPdf")]
    public async Task<IActionResult> ExportMyPdf(DateTimeOffset date)
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return Unauthorized();

        var month = await _monthDayService.GetMonth(date, userId.Value);
        if (month == null) return NotFound();

        var pdfBytes = _pdfService.GeneratePdf(month);
        return File(pdfBytes, "application/pdf", $"WorkLog_{date:yyyy-MM}.pdf");
    }

    // ── Admin endpoints ──

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

    [Authorize(Roles = ApplicationRoles.Admin)]
    [HttpPost]
    [Route("SaveMonth")]
    public async Task<IActionResult> SaveMonth(IEnumerable<WorkDayViewModel> days, DateTimeOffset date, Guid userId)
    {
        await _monthDayService.SaveMonth(days, date, userId);
        return Ok();
    }

    [Authorize(Roles = ApplicationRoles.Admin)]
    [HttpGet]
    [Route("ExportPdf")]
    public async Task<IActionResult> ExportPdf(DateTimeOffset date, Guid userId)
    {
        var month = await _monthDayService.GetMonth(date, userId);
        if (month == null) return NotFound();

        var pdfBytes = _pdfService.GeneratePdf(month);
        return File(pdfBytes, "application/pdf", $"WorkLog_{date:yyyy-MM}.pdf");
    }
}
