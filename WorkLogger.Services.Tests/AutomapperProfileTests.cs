using AutoMapper;
using WorkLogger.Domain.Automapper;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Services;

namespace WorkLogger.Domain.Tests.AutomapperProfileTests;

public class AutomapperProfileTests
{
    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        var pdfService = new PdfService();

        var model = new UserWorkMonthViewModel
        {
            EmployeeSettings = new EmployeeSettings()
            {
                FullName = "aaa"
            },
            DateMonth = new DateTimeOffset(2024, 04, 09, 18, 00, 00, TimeSpan.Zero),
            Days = new List<WorkDayViewModel>()
            {
                new WorkDayViewModel
                {
                    Date = new DateTimeOffset(2024, 04, 07, 00, 00, 00, TimeSpan.Zero),
                },
                new WorkDayViewModel
                {
                    Date = new DateTimeOffset(2024, 04, 06, 00, 00, 00, TimeSpan.Zero),
                },
                new WorkDayViewModel
                {
                    Date = new DateTimeOffset(2024, 04, 09, 00, 00, 00, TimeSpan.Zero),
                    StartHour = TimeSpan.Zero,
                    EndHour = TimeSpan.MaxValue,
                    IsVacation = true
                },
                new WorkDayViewModel
                {
                    Date = new DateTimeOffset(2024, 04, 10, 00, 00, 00, TimeSpan.Zero),
                    StartHour = TimeSpan.Zero,
                    EndHour = TimeSpan.MaxValue,
                    IsDayOff = true
                },
                new WorkDayViewModel
                {
                    Date = new DateTimeOffset(2024, 04, 11, 00, 00, 00, TimeSpan.Zero),
                    StartHour = TimeSpan.Zero,
                    EndHour = TimeSpan.MaxValue
                }
            }
        };

        var content = pdfService.GeneratePdf(model);
        File.WriteAllBytes("hello.pdf", content);
    }
}
