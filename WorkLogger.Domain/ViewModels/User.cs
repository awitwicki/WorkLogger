using Microsoft.AspNetCore.Identity;

namespace WorkLogger.Domain.ViewModels;

public class UserViewModel
{
    public IdentityUser User { get; set; }

    public IEnumerable<string> UserRoles { get; set; } = new List<string>();
}
