using Microsoft.AspNetCore.Identity;

namespace WorkLogger.Domain.Entities;

public class MonthWorkDay
{
    public long Id { get; set; }

    public string EmployeeId { get; set; }
    public IdentityUser Employee { get; set; }

    public DateTimeOffset DateMonth { get; set; }
    public IEnumerable<MonthWorkDayItem> Days { get; set; }
}
