using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Services;

public interface IPdfService
{
    byte[] GeneratePdf(UserWorkMonthViewModel model);
}
