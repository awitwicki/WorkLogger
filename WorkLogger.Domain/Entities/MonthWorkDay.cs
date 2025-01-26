using Microsoft.AspNetCore.Identity;

namespace WorkLogger.Domain.Entities;

public class MonthWorkDay
{
    public long Id { get; set; }

    public Guid EmployeeId { get; set; }
    public ApplicationUser Employee { get; set; }
    public DateTimeOffset DateMonth { get; set; }
    public IEnumerable<MonthWorkDayItem> Days { get; set; }
}
