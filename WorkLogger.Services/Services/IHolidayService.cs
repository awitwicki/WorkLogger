using WorkLogger.Domain.Entities;

namespace WorkLogger.Services.Services;

public interface IHolidayService
{
    Task<List<Holiday>> GetHolidays();
    Task AddHoliday(DateOnly date, string name);
    Task<bool> RemoveHoliday(DateOnly date);
}
