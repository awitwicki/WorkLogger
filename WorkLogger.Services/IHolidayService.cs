using WorkLogger.Domain.Entities;

namespace WorkLogger.Services;

public interface IHolidayService
{
    Task<List<Holiday>> GetHolidays(DateTimeOffset? dateFrom = null, DateTimeOffset? dateTo = null);
    Task AddHoliday(DateOnly date, string name);
    Task ImportHolidays(IList<Holiday> holidaysToAdd);
    Task<bool> RemoveHoliday(DateOnly date);
}
