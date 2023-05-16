namespace WorkLogger.Domain.Entities;

public class MonthWorkDayItem
{
    public DateTimeOffset Date { get; set; }
    public TimeSpan? StartHour { get; set; }
    public TimeSpan? EndHour { get; set; }
    public bool IsVacation { get; set; }
    public bool IsDayOff { get; set; }
    public bool IsL4 { get; set; }
}
