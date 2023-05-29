using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Domain.PdfModels;

public class WorkingTimeRecordsDocument : IDocument 
{
    public UserWorkMonthViewModel Model { get; }
    
    public WorkingTimeRecordsDocument(UserWorkMonthViewModel model)
    {
        Model = model;
    }
    
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial));
                page.MarginVertical(20);
                page.MarginHorizontal(40);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
    }
    
    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold();
    
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Ewidencja czasu pracy {Model.DateMonth.Date:MM.yyyy}").Style(titleStyle);
        
                column.Item().Text(text =>
                {
                    text.Span($"{Model.EmployeeSettings.FullName}").FontSize(15).SemiBold();
                });
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container
            .PaddingVertical(10)
            .Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(70);
                    columns.ConstantColumn(70);
                    columns.ConstantColumn(70);
                    columns.RelativeColumn();
                });
                
                // step 2
                table.Header(header =>
                {
                    var textStyle = TextStyle.Default.SemiBold().FontColor(Colors.White);
                    
                    header.Cell().Element(CellStyle).Text(Model.DateMonth.Date.ToString("MM.yyyy")).Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Od").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Do").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Opis").Style(textStyle);
                    
                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                            .Border(1)
                            .BorderColor(Colors.Black)
                            .Background("#3662ae")
                            .PaddingLeft(5);
                    }
                });
                
                // step 3
                foreach (var day in Model.Days)
                {
                    var isWeekend = !(day.IsVacation || day.IsDayOff);
                    
                    table.Cell().Element(x => isWeekend ? CellStyle(x) : CellStyleWeekend(x)).Text(day.Date.ToString("dd.MM"));
                    table.Cell().Element(x => isWeekend ? CellStyle(x) : CellStyleWeekend(x)).Text(!(day.IsVacationOrL4 || day.IsDayOff) ? day.StartHour?.ToString(@"hh\:mm") : "");
                    table.Cell().Element(x => isWeekend ? CellStyle(x) : CellStyleWeekend(x)).Text(!(day.IsVacationOrL4 || day.IsDayOff) ? day.EndHour?.ToString(@"hh\:mm") : "");
                    table.Cell().Element(x => isWeekend ? CellStyle(x) : CellStyleWeekend(x)).Text("");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                            .Border(1)
                            .BorderColor(Colors.Black)
                            .PaddingVertical(2)
                            .PaddingLeft(5);
                    }
                    
                    static IContainer CellStyleWeekend(IContainer container)
                    {
                        return container
                            .Background("#b3ceff")
                            .Element(CellStyle);
                    }
                }
            });
    }
    
    void ComposeFooter(IContainer container)
    {
        container
            .Height(50)
            .Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(Model.EmployeeSettings.FullName);
                });
                
                row.RelativeItem().Column(column =>
                {
                    column.Item().AlignRight().Text($"Zatwierdził:");
                });
            });
    }
}