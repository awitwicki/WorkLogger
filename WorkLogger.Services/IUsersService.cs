using Microsoft.AspNetCore.Identity;
using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Services;

public interface IUsersService
{
    Task<List<UserViewModel>> GetUsers();
    Task AddRoleToUser(Guid userId, string roleName);
    Task<IEnumerable<string>> GetUserRoles(Guid userId);
    Task CleanUserRoles(Guid userId);
    Task RemoveUser(Guid userId);
}
