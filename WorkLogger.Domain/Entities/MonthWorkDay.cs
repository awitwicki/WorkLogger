using Microsoft.AspNetCore.Identity;

namespace WorkLogger.Domain.Entities;

public class MonthWorkDay
{
    public long Id { get; set; }

    // User
    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public DateTime DateMonth { get; set; }
    public IEnumerable<MonthWorkDayItem> Days { get; set; }
}

