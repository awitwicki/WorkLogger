using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services.Services;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _dbContext;
    
    public UsersService(ApplicationDbContext context)
    {
        _dbContext = context;
    }
        
    public async Task<List<UserViewModel>> GetUsers()
    {
        var queryUsersAndRoles = from usr in _dbContext.Users
            join usrRoles in _dbContext.UserRoles on usr.Id equals usrRoles.UserId
                into groupingUserRoles
            from usrRoles in groupingUserRoles.DefaultIfEmpty()
            join roles in _dbContext.Roles on usrRoles.RoleId equals roles.Id
                into groupingRoles
            from roles in groupingRoles.DefaultIfEmpty()
            select new { User = usr, UserRoles = roles };
       
        var normalizedUsersViewModel = await queryUsersAndRoles.GroupBy(x => x.User.Id)
            .Select(x => new UserViewModel
            {
                User = x.First().User,
                UserRoles = x.Select(r => r.UserRoles.Name!).Distinct().ToList()
            })
            .ToListAsync();

        return normalizedUsersViewModel;
    }
}
