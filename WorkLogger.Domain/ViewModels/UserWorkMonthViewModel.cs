using Microsoft.AspNetCore.Identity;

namespace WorkLogger.Domain.ViewModels;

public class UserWorkMonthViewModel
{
    public long Id { get; set; }

    public string EmployeeId { get; set; }
    public IdentityUser Employee { get; set; }
    public DateTimeOffset DateMonth { get; set; }
    public IEnumerable<WorkDayViewModel> Days { get; set; }
}
