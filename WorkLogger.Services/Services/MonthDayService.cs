using System.Collections.Immutable;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Common.DateExtensions;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Services;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services.Services;

public class MonthDayService : IMonthDayService
{
    private readonly IHolidayService _holidayService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public MonthDayService(IHolidayService holidayService, ApplicationDbContext context, IMapper mapper)
    {
        _holidayService = holidayService;
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<MonthDayFormItem>> BuildMonth(DateTimeOffset date)
    {
        // Truncate date to month
        date = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var days = DateTime.DaysInMonth(date.Year, date.Month);
        
        var dateFrom = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var dateTo = new DateTimeOffset(date.Year, date.Month, days, 0, 0, 0, TimeSpan.Zero);

        var holidays = (await _holidayService.GetHolidays(dateFrom, dateTo))
            .ToImmutableDictionary(x => x.DateDay.ToDateOnly());
        
        var month = new List<MonthDayFormItem>();
        for (var i = 1; i <= days; i++)
        {
            date = new DateTime(date.Year, date.Month, i);
            var isHoliday = holidays.TryGetValue(date.ToDateOnly(), out var holidayValue);
            
            month.Add(new MonthDayFormItem
            {
                Date = date,
                StartHour = TimeSpan.FromHours(8),
                EndHour = TimeSpan.FromHours(16),
                IsVacation = false,
                Holiday = isHoliday ? holidayValue : null
            });

            month.Last().IsDayOff = month.Last().Date.DayOfWeek == DayOfWeek.Saturday ||
                                    month.Last().Date.DayOfWeek == DayOfWeek.Sunday ||
                                    holidays.ContainsKey(date.ToDateOnly());
        }

        return month;
    }

    public async ValueTask<IEnumerable<MonthDayFormItem>> GetMonthDays(DateTimeOffset date, string userId)
    {
        // Truncate date to month
        date = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);

        IEnumerable<MonthDayFormItem> monthDays;

        var month = await _context.MonthWorkDays.AsNoTracking()
            .Where(x => x.DateMonth == date)
            .Where(x => x.EmployeeId == userId)
            .FirstOrDefaultAsync();

        if (month != null)
        {
            return _mapper.Map<IEnumerable<MonthDayFormItem>>(month.Days);
        }

        return null;
    }

    public Task<MonthWorkDay> GetMonth(DateTimeOffset date, string userId)
    {
        // Truncate date to month
        date = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
        
        return _context.MonthWorkDays.AsNoTracking()
                .Where(x => x.DateMonth == date)
                .Where(x => x.EmployeeId == userId)
                .Include(x => x.Employee)
                .FirstOrDefaultAsync();
    }

    public async Task SaveMonth(IEnumerable<MonthDayFormItem> days, DateTimeOffset date, string userId)
    {
        // Truncate date to month
        date = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);

        MonthWorkDay? month;
        
        // Add or Update
        month = await _context.MonthWorkDays.AsNoTracking()
            .Where(x => x.DateMonth == date)
            .Where(x => x.EmployeeId == userId)
            .FirstOrDefaultAsync();

        if (month != null)
        {
            month.Days = _mapper.Map<List<MonthWorkDayItem>>(days);
            
            _context.MonthWorkDays.Update(month);
        }
        else
        {
            month = new MonthWorkDay
            {
                DateMonth = date,
                EmployeeId = userId,
                Days = _mapper.Map<List<MonthWorkDayItem>>(days)
            };
            
            _context.MonthWorkDays.Add(month);
        }
        
        await _context.SaveChangesAsync();
        
        _context.Entry(month).State = EntityState.Detached;
    }

    public ValueTask<IEnumerable<MonthWorkDay>> GetUserMonths(string userId)
    {
        var months = _context.MonthWorkDays.AsNoTracking()
            .Where(x => x.EmployeeId == userId)
            .Include(x => x.Employee)
            .OrderBy(x => x.DateMonth);

        return ValueTask.FromResult(months.AsEnumerable());
    }
}
