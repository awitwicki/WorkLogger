using Microsoft.AspNetCore.Identity;

namespace WorkLogger.Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>;

public static class ApplicationRoles
{
    public const string Admin = "admin";
    public const string Moderator = "moderator";
    public const string UserRole = "user";
}
