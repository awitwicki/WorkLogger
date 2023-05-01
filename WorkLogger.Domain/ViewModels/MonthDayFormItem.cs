namespace WorkLogger.Domain.ViewModels;

public class MonthDayFormItem
{
    public DateTimeOffset Date { get; set; }
    public TimeSpan? StartHour { get; set; }
    public TimeSpan? EndHour { get; set; }
    public bool IsVacation { get; set; }
    public bool IsDayOff { get; set; }
    public bool IsDisabled => IsVacation || IsDayOff;
}
