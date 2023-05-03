using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Domain.Services;

public interface IMonthDayService
{
    ValueTask<IEnumerable<MonthDayFormItem>> BuildMonth(DateTimeOffset date);
    ValueTask<IEnumerable<MonthDayFormItem>> GetMonthDays(DateTimeOffset date, string userId);
    Task<MonthWorkDay> GetMonth(DateTimeOffset date, string userId);
    Task SaveMonth(IEnumerable<MonthDayFormItem> days, DateTimeOffset date, string userId);
    ValueTask<IEnumerable<MonthWorkDay>> GetUserMonths(string userId);
}
