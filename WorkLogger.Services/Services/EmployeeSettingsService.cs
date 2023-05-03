using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services.Services;

public class EmployeeSettingsService : IEmployeeSettingsService
{
    private readonly ApplicationDbContext _dbContext;
    
    public EmployeeSettingsService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<EmployeeSettings> GetEmployeeSettings(string employeeId)
    {
        return _dbContext.EmployeeSettings.AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
    }

    public async Task SaveOrUpdateEmployeeSettings(EmployeeSettings employeeSettings)
    {
        _dbContext.EmployeeSettings.Update(employeeSettings);
        
        var user = await _dbContext.Users.FirstAsync(x => x.Id == employeeSettings.EmployeeId);
        user.UserName = employeeSettings.FullName;
        
        await _dbContext.SaveChangesAsync();

        _dbContext.Entry(employeeSettings).State = EntityState.Detached;
    }
}
