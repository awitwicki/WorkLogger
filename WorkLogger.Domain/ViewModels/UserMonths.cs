using WorkLogger.Domain.Entities;

namespace WorkLogger.Domain.ViewModels;

public class UserMonthsViewModel
{
    public UserViewModel User { get; set; }
    public IEnumerable<MonthWorkDay> UserWorkMonths { get; set; }
    
    public MonthWorkDay? SelectedMonth { get; set; }
}
