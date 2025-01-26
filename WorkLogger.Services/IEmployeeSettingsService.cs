using WorkLogger.Domain.Entities;

namespace WorkLogger.Services;

public interface IEmployeeSettingsService
{
    Task<EmployeeSettings> GetEmployeeSettings(Guid employeeId);
    Task SaveOrUpdateEmployeeSettings(EmployeeSettings employeeSettings);
}
