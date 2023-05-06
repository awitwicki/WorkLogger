using Microsoft.AspNetCore.Identity;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Domain.ViewModels;

public class UserViewModel
{
    public IdentityUser User { get; set; }

    public IEnumerable<string> UserRoles { get; set; } = new List<string>();
    
    public EmployeeSettings EmployeeSettings { get; set; }
}
