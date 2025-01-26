using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers.User;

[Route("[controller]")]
[Authorize(Roles = ApplicationRoles.Admin)]
public class DashboardController: ControllerBase
{
    private readonly IHolidayService _holidayService;

    public DashboardController(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        //var result = await _holidayService.GetHolidays();
        var result = new DashboardViewModel();
        
        return Ok(result);
    }
}
