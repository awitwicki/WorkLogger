using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Services.Services;

public interface IPdfService
{
    byte[] GeneratePdf(UserWorkMonthViewModel model);
}
