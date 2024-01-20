using Microsoft.EntityFrameworkCore;
using WorkLogger.Common.DateExtensions;
using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services;

public class HolidayService : IHolidayService
{
    private readonly ApplicationDbContext _dbContext;
    
    public HolidayService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Holiday>> GetHolidays(DateTimeOffset? dateFrom = null, DateTimeOffset? dateTo = null)
    {
        var query = _dbContext.Holidays.AsNoTracking();

        if (dateFrom.HasValue)
        {
            query = query.Where(x => x.DateDay >= dateFrom);
        }

        if (dateTo.HasValue)
        {
            query = query.Where(x => x.DateDay <= dateTo);
        }

        return query.OrderBy(x => x.DateDay).ToListAsync();
    }

    public async Task AddHoliday(DateOnly date, string name)
    {
        var entity = _dbContext.Holidays.Add(
            new Holiday
            {
                DateDay = new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero),
                Name = name
            });
        
        await _dbContext.SaveChangesAsync();
        
        _dbContext.Entry(entity.Entity).State = EntityState.Detached;
    }

    public async Task ImportHolidays(IList<Holiday> holidaysToAdd)
    {
        var holidaysInDb = await _dbContext.Holidays.AsNoTracking().ToListAsync();
        var holidaysInDbDates = holidaysInDb.Select(x => x.DateDay).ToList();

        holidaysToAdd = holidaysToAdd.Where(x => !holidaysInDbDates.Contains(x.DateDay)).ToList();
        await _dbContext.AddRangeAsync(holidaysToAdd);

        await _dbContext.SaveChangesAsync();
        
        // Stop tracking
        foreach (var newHoliday in holidaysToAdd)
        {
            _dbContext.Entry(newHoliday).State = EntityState.Detached;
        }
    }

    public async Task<bool> RemoveHoliday(DateOnly date)
    {
        var holiday = await _dbContext.Holidays.AsNoTracking()
            .FirstOrDefaultAsync(x => x.DateDay == date.ToDateTimeOffset());

        if (holiday != null)
        {
            _dbContext.Holidays.Remove(holiday);

            await _dbContext.SaveChangesAsync();
            
            _dbContext.Entry(holiday).State = EntityState.Detached;

            return true;
        }
        else
        {
            _dbContext.Entry(holiday).State = EntityState.Detached;
            return false;
        }
    }
}
