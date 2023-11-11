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
    
    public async Task<EmployeeSettings> GetEmployeeSettings(string employeeId)
    {
        var result = await _dbContext.EmployeeSettings.AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

        if (result != null)
        {
            _dbContext.Entry(result).State = EntityState.Detached;
        }
        
        return result;
    }

    public async Task SaveOrUpdateEmployeeSettings(EmployeeSettings employeeSettings)
    {
        _dbContext.EmployeeSettings.Update(employeeSettings);
        
        await _dbContext.SaveChangesAsync();

        _dbContext.Entry(employeeSettings).State = EntityState.Detached;
    }
}
