using System.Security.Claims;

namespace WorkLogger.Common.AspExtensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value!;
    }
}
