using QuestPDF.Fluent;
using WorkLogger.Domain.PdfModels;
using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Services.Services;

public class PdfService : IPdfService
{
    public byte[] GeneratePdf(UserWorkMonthViewModel model)
    {
        var container = new WorkingTimeRecordsDocument(model);
        
        var bytesFile = container.GeneratePdf();

        return bytesFile;
    }
}
