using Microsoft.EntityFrameworkCore;
using WorkLogger.Infrastructure.Database;

namespace WorkLogger.Services;

public class VacationService : IVacationService
{
    private readonly ApplicationDbContext _dbContext;

    public VacationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> GetVacationDaysLeftForUser(Guid employeeId)
    {
        var resultEmployeeSettings = await _dbContext.EmployeeSettings.AsNoTracking()
            .FirstAsync(x => x.EmployeeId == employeeId);

        var contractStartDate = resultEmployeeSettings.ContractStartedDate;
        var contractContVacationsPerYear = resultEmployeeSettings.VacationDaysPerYear;

        var contractSignetMonth = contractStartDate.Date.Month;
        var contractSignetDay = contractStartDate.Date.Day;

        var actualContractDateInThisYear = new DateTime(DateTime.UtcNow.Year, contractSignetMonth, contractSignetDay, 0,
            0, 0, DateTimeKind.Utc);
        if (actualContractDateInThisYear > DateTime.UtcNow)
            actualContractDateInThisYear.AddYears(-1);

        // Get logged workdays
        var workDays = await _dbContext.MonthWorkDays.AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Where(x => x.DateMonth >= contractStartDate)
            .ToListAsync();

        var workDaysCount = workDays
            .SelectMany(x => x.Days)
            .Count(x => x.IsVacation);

        var vacationDaysLeft = contractContVacationsPerYear - workDaysCount;

        return vacationDaysLeft;
    }
}
