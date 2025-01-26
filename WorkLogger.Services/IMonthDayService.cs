using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Services;

public interface IMonthDayService
{
    Task<IEnumerable<WorkDayViewModel>> BuildMonth(DateTimeOffset date);
    Task<IEnumerable<WorkDayViewModel>> GetDaysInMonth(DateTimeOffset date, Guid userId);
    Task<UserWorkMonthViewModel> GetMonth(DateTimeOffset date, Guid userId);
    Task SaveMonth(IEnumerable<WorkDayViewModel> days, DateTimeOffset date, Guid userId);
    Task<IEnumerable<UserWorkMonthViewModel>> GetUserMonths(Guid userId);
}
