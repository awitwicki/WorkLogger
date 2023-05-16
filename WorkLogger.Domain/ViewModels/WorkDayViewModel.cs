using WorkLogger.Domain.Entities;

namespace WorkLogger.Domain.ViewModels;

public class WorkDayViewModel
{
    public DateTimeOffset Date { get; set; }
    public TimeSpan? StartHour { get; set; }
    public TimeSpan? EndHour { get; set; }
    public bool IsVacation { get; set; }
    public bool IsDayOff { get; set; }
    public bool IsL4 { get; set; }
    public bool IsVacationOrL4 => IsVacation || IsL4;
    public Holiday? Holiday { get; set; }
    public bool IsDisabled => IsVacation || IsDayOff || IsL4;
}
