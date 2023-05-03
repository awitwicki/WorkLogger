using WorkLogger.Domain.Entities;

namespace WorkLogger.Services.Services;

public interface IPdfService
{
    byte[] GeneratePdf(MonthWorkDay model);
}
