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
    
    public async Task<IEnumerable<WorkDayViewModel>> BuildMonth(DateTimeOffset date)
    {
        // Truncate date to month
        date = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var days = DateTime.DaysInMonth(date.Year, date.Month);
        
        var dateFrom = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var dateTo = new DateTimeOffset(date.Year, date.Month, days, 0, 0, 0, TimeSpan.Zero);

        var holidays = (await _holidayService.GetHolidays(dateFrom, dateTo))
            .ToImmutableDictionary(x => x.DateDay.ToDateOnly());
        
        var month = new List<WorkDayViewModel>();
        for (var i = 1; i <= days; i++)
        {
            date = new DateTime(date.Year, date.Month, i);
            var isHoliday = holidays.TryGetValue(date.ToDateOnly(), out var holidayValue);
            
            month.Add(new WorkDayViewModel
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

    public async Task<IEnumerable<WorkDayViewModel>> GetDaysInMonth(DateTimeOffset date, string userId)
    {
        // Truncate date to month
        date = date.TruncateToMonth();

        IEnumerable<WorkDayViewModel> monthDays;

        var month = await _context.MonthWorkDays.AsNoTracking()
            .Where(x => x.DateMonth == date)
            .Where(x => x.EmployeeId == userId)
            .FirstOrDefaultAsync();
        
        if (month != null)
        {
            var holidays = (await _holidayService.GetHolidays(date, date.AddMonths(1).AddDays(-1)))
                .ToImmutableDictionary(x => x.DateDay.ToDateOnly());
            
            var workDaysViewModel  = _mapper.Map<IEnumerable<WorkDayViewModel>>(month.Days);
            
            // Add holidays
            foreach (var workDayViewModel in workDaysViewModel)
            {
                if (holidays.TryGetValue(workDayViewModel.Date.ToDateOnly(), out var holidayValue))
                {
                    workDayViewModel.Holiday = holidayValue;
                }
            }

            return workDaysViewModel;
        }

        return null;
    }

    public async Task<UserWorkMonthViewModel> GetMonth(DateTimeOffset date, string userId)
    {
        // Truncate date to month
        date = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
        
        var month = await _context.MonthWorkDays.AsNoTracking()
                .Where(x => x.DateMonth == date)
                .Where(x => x.EmployeeId == userId)
                .Include(x => x.Employee)
                .FirstOrDefaultAsync();
        
        return _mapper.Map<UserWorkMonthViewModel>(month);
    }

    public async Task SaveMonth(IEnumerable<WorkDayViewModel> days, DateTimeOffset date, string userId)
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

    public async Task<IEnumerable<UserWorkMonthViewModel>> GetUserMonths(string userId)
    {
        var months = await _context.MonthWorkDays.AsNoTracking()
            .Where(x => x.EmployeeId == userId)
            .Include(x => x.Employee)
            .OrderBy(x => x.DateMonth)
            .ToListAsync();

        var monthViewModelCollection = _mapper.Map<IEnumerable<UserWorkMonthViewModel>>(months);

        var holidays = (await _holidayService.GetHolidays())
            .ToImmutableDictionary(x => x.DateDay.ToDateOnly());
        
        // Add holidays
        foreach (var monthViewModel in monthViewModelCollection)
        {
            foreach (var day in monthViewModel.Days)
            {
                if (holidays.TryGetValue(day.Date.ToDateOnly(), out var holidayValue))
                {
                    day.Holiday = holidayValue;
                }
            }
        }

        return monthViewModelCollection;
    }
}
