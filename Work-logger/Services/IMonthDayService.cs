using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Domain.Services;

public interface IMonthDayService
{
    public ValueTask<IEnumerable<MonthDayFormItem>> BuildMonth(DateTime date);
}
