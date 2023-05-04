using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Database;
using WorkLogger.Common.DateExtensions;

namespace WorkLogger.Services.Services;

public class HolidayService : IHolidayService
{
    private readonly ApplicationDbContext _dbContext;
    
    public HolidayService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<List<Holiday>> GetHolidays()
    {
        return _dbContext.Holidays.AsNoTracking().ToListAsync();
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
