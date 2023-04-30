﻿using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Domain.Services;

public class MonthDayService : IMonthDayService
{
    public ValueTask<IEnumerable<MonthDayFormItem>> BuildMonth(DateTime date)
    {
        var days = DateTime.DaysInMonth(date.Year, date.Month);
        var month = new List<MonthDayFormItem>();
        for (var i = 1; i <= days; i++)
        {
            month.Add(new MonthDayFormItem
            {
                Date = new DateTime(date.Year, date.Month, i),
                Start = TimeSpan.FromHours(8),
                End = TimeSpan.FromHours(16),
                IsVacation = false,
            });
            
            month.Last().IsDayOff = month.Last().Date.DayOfWeek == DayOfWeek.Saturday ||
                                       month.Last().Date.DayOfWeek == DayOfWeek.Sunday;
        }

        return ValueTask.FromResult((IEnumerable<MonthDayFormItem>)month);
    }
}
