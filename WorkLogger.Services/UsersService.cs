using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UsersService> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public UsersService(ApplicationDbContext dbContext, ILogger<UsersService> logger, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userManager = userManager;
    }

    private List<String> GetUserRoles(string userId, ICollection<IdentityRole> roles,
        ICollection<IdentityUserRole<string>> userRoles)
    {
        var assignedUserRoles = userRoles.Where(y => y.UserId == userId).ToList();
        var assignedUserRolesId = assignedUserRoles.Select(x => x.RoleId).ToList();

        var filteredUerRoles = roles.Where(x => assignedUserRolesId.Contains(x.Id)).ToList();

        return filteredUerRoles.Select(x => x.Name).ToList();
    }
    
    public async Task<List<UserViewModel>> GetUsers()
    {
        var users = await _dbContext.Users.AsNoTracking().ToListAsync();
        var employeeSettings = await _dbContext.EmployeeSettings.AsNoTracking().ToListAsync();
        var roles = await _dbContext.Roles.AsNoTracking().ToListAsync();
        var userRoles = await _dbContext.UserRoles.AsNoTracking().ToListAsync();

        var result = users.Select(x => new UserViewModel
            {
                User = x,
                UserRoles = GetUserRoles(x.Id, roles, userRoles),
                EmployeeSettings = employeeSettings.FirstOrDefault(y => y.EmployeeId == x.Id)
            })
            .ToList();

        return result;
    }
    
    public async Task AddRoleToUser(string userId, string roleName)
    {
        try
        {
            var role = await _dbContext.Roles.AsNoTracking().FirstAsync(x => x.Name == roleName);

            var identityUserRole = new IdentityUserRole<string>
            {
                UserId = userId,
                RoleId = role.Id
            };
            
            await _dbContext.UserRoles.AddAsync(identityUserRole);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't add role to user");
            throw;
        }
    }

    public async Task<IEnumerable<string>> GetUserRoles(string userId)
    {
        IdentityUser user = new IdentityUser { Id = userId };

        var userRoles = await _userManager.GetRolesAsync(user);
        
        _dbContext.Entry(user).State = EntityState.Detached;
        return userRoles;
    }

    public async Task CleanUserRoles(string userId)
    {
        var rolesToRemove = await _dbContext.UserRoles.Where(x => x.UserId == userId).ToListAsync();
        _dbContext.UserRoles.RemoveRange(rolesToRemove);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveUser(string userId)
    {
        var user = await _dbContext.Users.Where(x => x.Id == userId).FirstAsync();
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<IdentityRole>> GetAllRoles()
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .ToListAsync();
    }
}
