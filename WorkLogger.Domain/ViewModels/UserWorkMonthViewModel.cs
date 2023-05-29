using Microsoft.AspNetCore.Identity;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Domain.ViewModels;

public class UserWorkMonthViewModel
{
    public long Id { get; set; }

    public string EmployeeId { get; set; }
    public EmployeeSettings EmployeeSettings { get; set; }
    public DateTimeOffset DateMonth { get; set; }
    public IEnumerable<WorkDayViewModel> Days { get; set; }
}
