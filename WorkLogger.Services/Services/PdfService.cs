using Microsoft.AspNetCore.Http;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.PdfModels;

namespace WorkLogger.Services.Services;

public class PdfService : IPdfService
{
    public byte[] GeneratePdfAsync(MonthWorkDay model)
    {
        var container = new WorkingTimeRecordsDocument(model);
        
        var bytesFile = container.GeneratePdf();

        return bytesFile;
    }
}
