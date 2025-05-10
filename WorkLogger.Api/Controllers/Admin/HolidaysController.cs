using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Common;
using WorkLogger.Common.DateExtensions;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers.Admin;

[ApiController]
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
    public async Task<IActionResult> AddHoliday([FromBody] HolidayDto dto)
    {
        await _holidayService.AddHoliday(dto.Date.ToDateOnly(), dto.Name);
        
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
    public async Task<IActionResult> RemoveHoliday([FromBody] DateTime date)
    {
        var result = await _holidayService.RemoveHoliday(date.ToDateOnly());
        
        return Ok(new { wasInDatabase = result});
    }
}
