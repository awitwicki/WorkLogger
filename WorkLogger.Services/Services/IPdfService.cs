using WorkLogger.Domain.Entities;

namespace WorkLogger.Services.Services;

public interface IPdfService
{
    byte[] GeneratePdfAsync(MonthWorkDay model);
}
