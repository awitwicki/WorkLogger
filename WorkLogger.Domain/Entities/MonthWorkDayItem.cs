namespace WorkLogger.Domain.Entities;

public class MonthWorkDayItem
{
    public DateTime Date { get; set; }
    public TimeSpan? StartHour { get; set; }
    public TimeSpan? EndHour { get; set; }
    public bool IsVacation { get; set; }
    public bool IsDayOff { get; set; }
}
