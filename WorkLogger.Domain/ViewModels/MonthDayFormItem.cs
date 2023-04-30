namespace WorkLogger.Domain.ViewModels;

public class MonthDayFormItem
{
    public DateTime Date { get; set; }
    public TimeSpan? Start { get; set; }
    public TimeSpan? End { get; set; }
    public bool IsVacation { get; set; }
    public bool IsDayOff { get; set; }
    public bool IsDisabled => IsVacation || IsDayOff;
}
