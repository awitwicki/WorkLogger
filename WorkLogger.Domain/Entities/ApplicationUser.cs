using Microsoft.AspNetCore.Identity;

namespace WorkLogger.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedOn { get; set; }
}
