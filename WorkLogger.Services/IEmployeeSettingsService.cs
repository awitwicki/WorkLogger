using WorkLogger.Domain.Entities;

namespace WorkLogger.Services;

public interface IEmployeeSettingsService
{
    Task<EmployeeSettings> GetEmployeeSettings(string employeeId);
    Task SaveOrUpdateEmployeeSettings(EmployeeSettings employeeSettings);
}
