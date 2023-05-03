using QuestPDF.Fluent;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.PdfModels;

namespace WorkLogger.Services.Services;

public class PdfService : IPdfService
{
    public byte[] GeneratePdf(MonthWorkDay model)
    {
        var container = new WorkingTimeRecordsDocument(model);
        
        var bytesFile = container.GeneratePdf();

        return bytesFile;
    }
}
