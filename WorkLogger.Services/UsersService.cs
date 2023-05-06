using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services.Services;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _dbContext;
    public UserManager<IdentityUser> _userManager { get; set; }
    
    public UsersService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    
    public async Task<List<UserViewModel>> GetUsers()
    {
        var queryUsersAndRoles = from usr in _dbContext.Users
            join usrRoles in _dbContext.UserRoles on usr.Id equals usrRoles.UserId
                into groupingUserRoles
            from usrRoles in groupingUserRoles.DefaultIfEmpty()
           
            join settings in _dbContext.EmployeeSettings on usr.Id equals settings.EmployeeId
                into groupingsettings
            from settings in groupingsettings.DefaultIfEmpty()
            
            join roles in _dbContext.Roles on usrRoles.RoleId equals roles.Id
                into groupingRoles
            from roles in groupingRoles.DefaultIfEmpty()
            select new { User = usr, UserRoles = roles, EmployeeSettings = settings  };
       
        var normalizedUsersViewModel = await queryUsersAndRoles.GroupBy(x => x.User.Id)
            .Select(x => new UserViewModel
            {
                User = x.First().User,
                UserRoles = x.Select(r => r.UserRoles.Name!).Distinct().ToList(),
                EmployeeSettings = x.First().EmployeeSettings
            })
            .ToListAsync();

        normalizedUsersViewModel
            .Where(x => x.EmployeeSettings != null)
            .ToList()
            .ForEach(x =>
                _dbContext.Entry(x.EmployeeSettings).State = EntityState.Detached);

        return normalizedUsersViewModel;
    }
    
    public async Task AddRoleToUser(string userId, string role)
    {
        IdentityUser user = new IdentityUser { Id = userId };

        await _userManager.AddToRoleAsync(user, role);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetUserRoles(string userId)
    {
        IdentityUser user = new IdentityUser { Id = userId };

        var userRoles = await _userManager.GetRolesAsync(user);

        return userRoles;
    }

    public async Task CleanUserRoles(string userId)
    {
        IdentityUser user = new IdentityUser { Id = userId };

        var userRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(new IdentityUser { Id = userId }, userRoles);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<IdentityRole>> GetAllRoles()
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .ToListAsync();
    }
}
