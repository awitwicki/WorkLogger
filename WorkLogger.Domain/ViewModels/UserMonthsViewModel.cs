namespace WorkLogger.Domain.ViewModels;

public class UserMonthsViewModel
{
    public UserViewModel User { get; set; }
    public IEnumerable<UserWorkMonthViewModel> UserWorkMonths { get; set; }
    public UserWorkMonthViewModel? SelectedMonth { get; set; }
}
