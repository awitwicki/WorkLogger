using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Services;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services.Services;

public class MonthDayService : IMonthDayService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public MonthDayService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public ValueTask<IEnumerable<MonthDayFormItem>> BuildMonth(DateTimeOffset date)
    {
        var days = DateTime.DaysInMonth(date.Year, date.Month);
        var month = new List<MonthDayFormItem>();
        for (var i = 1; i <= days; i++)
        {
            month.Add(new MonthDayFormItem
            {
                Date = new DateTime(date.Year, date.Month, i),
                StartHour = TimeSpan.FromHours(8),
                EndHour = TimeSpan.FromHours(16),
                IsVacation = false,
            });
            
            month.Last().IsDayOff = month.Last().Date.DayOfWeek == DayOfWeek.Saturday ||
                                       month.Last().Date.DayOfWeek == DayOfWeek.Sunday;
        }

        return ValueTask.FromResult((IEnumerable<MonthDayFormItem>)month);
    }

    public async ValueTask<IEnumerable<MonthDayFormItem>> GetMonthDays(DateTimeOffset date, string userId)
    {
        // Truncate date to month
        date = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
        
        IEnumerable<MonthDayFormItem> monthDays;

        try
        {
            var month = await _context.MonthWorkDays.AsNoTracking()
                .Where(x => x.DateMonth == date)
                .Where(x => x.EmployeeId == userId)
                .FirstOrDefaultAsync();
            
            if (month == null)
            {
                monthDays = BuildMonth(date).Result;
            }
            else
            {
                monthDays = _mapper.Map<IEnumerable<MonthDayFormItem>>(month.Days);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return monthDays;
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
                Days = _mapper.Map<List<MonthWorkDayItem>>(days),
            };
            
            _context.MonthWorkDays.Add(month);
        }
        
        await _context.SaveChangesAsync();
        
        // TODO deattach entity
    }

    public ValueTask<IEnumerable<MonthWorkDay>> GetUserMonths(string userId)
    {
        var months = _context.MonthWorkDays.AsNoTracking()
            .Where(x => x.EmployeeId == userId)
            .OrderByDescending(x => x.DateMonth);

        return ValueTask.FromResult(months.AsEnumerable());
    }
}
