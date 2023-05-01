using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Domain.Services;

public interface IMonthDayService
{
    ValueTask<IEnumerable<MonthDayFormItem>> BuildMonth(DateTimeOffset date);
    ValueTask<IEnumerable<MonthDayFormItem>> GetMonth(DateTimeOffset date, string userId);
    Task SaveMonth(IEnumerable<MonthDayFormItem> days, DateTimeOffset date, string userId);
}
