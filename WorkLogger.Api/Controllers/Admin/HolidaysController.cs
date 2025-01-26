using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Common;
using WorkLogger.Domain.Entities;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers.Admin;

[Route("[controller]")]
[Authorize(Roles = ApplicationRoles.Admin)]
public class HolidaysController: ControllerBase
{
    private readonly IHolidayService _holidayService;

    public HolidaysController(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> HolidaysList()
    {
        var holidays = await _holidayService.GetHolidays();
        
        return Ok(holidays);
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddHoliday(DateOnly date, string name)
    {
        await _holidayService.AddHoliday(date, name);
        
        return Ok();
    }
    
    [HttpPost]
    [Route("importHolidays")]
    public async Task<IActionResult> ImportHolidays()
    {
        var holidaysToAdd = HolidaysHelpers.GetHolidays2024();
        await _holidayService.ImportHolidays(holidaysToAdd);
        
        return Ok();
    }
    
    [HttpPost]
    [Route("remove")]
    public async Task<IActionResult> RemoveHoliday(DateOnly date)
    {
        var result = await _holidayService.RemoveHoliday(date);
        
        return Ok(new { wasInDatabase = result});
    }
}
