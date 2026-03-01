using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services;

public class EmployeeSettingsService : IEmployeeSettingsService
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeSettingsService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EmployeeSettings> GetEmployeeSettings(Guid employeeId)
    {
        var result = await _dbContext.EmployeeSettings.AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

        return result;
    }

    public async Task SaveOrUpdateEmployeeSettings(EmployeeSettings employeeSettings)
    {
        var existing = await _dbContext.EmployeeSettings
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeSettings.EmployeeId);

        if (existing != null)
        {
            existing.FullName = employeeSettings.FullName;
            existing.ContractStartedDate = employeeSettings.ContractStartedDate;
            existing.VacationDaysPerYear = employeeSettings.VacationDaysPerYear;
            existing.IsRequiredToFill = employeeSettings.IsRequiredToFill;
            _dbContext.EmployeeSettings.Update(existing);
        }
        else
        {
            _dbContext.EmployeeSettings.Add(employeeSettings);
        }

        await _dbContext.SaveChangesAsync();
    }
}
