using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Services;

public interface IMonthDayService
{
    Task<IEnumerable<WorkDayViewModel>> BuildMonth(DateTimeOffset date);
    Task<IEnumerable<WorkDayViewModel>> GetDaysInMonth(DateTimeOffset date, string userId);
    Task<UserWorkMonthViewModel> GetMonth(DateTimeOffset date, string userId);
    Task SaveMonth(IEnumerable<WorkDayViewModel> days, DateTimeOffset date, string userId);
    Task<IEnumerable<UserWorkMonthViewModel>> GetUserMonths(string userId);
}
