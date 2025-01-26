namespace WorkLogger.Services;

public interface IVacationService
{
    Task<int> GetVacationDaysLeftForUser(Guid employeeId);
}
