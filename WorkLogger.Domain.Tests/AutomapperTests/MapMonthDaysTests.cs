using AutoMapper;
using WorkLogger.Domain.Automapper;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Domain.Tests.AutomapperProfileTests;

public class MapMonthDaysTests
{
    IEnumerable<WorkDayViewModel> BuildMonth()
    {
        var date = DateTimeOffset.Now;
        
        var days = DateTime.DaysInMonth(date.Year, date.Month);
        var month = new List<WorkDayViewModel>();
        for (var i = 1; i <= days; i++)
        {
            month.Add(new WorkDayViewModel
            {
                Date = new DateTime(date.Year, date.Month, i),
                StartHour = TimeSpan.FromHours(8),
                EndHour = TimeSpan.FromHours(16),
                IsVacation = false,
            });
            
            month.Last().IsDayOff = month.Last().Date.DayOfWeek == DayOfWeek.Saturday ||
                                    month.Last().Date.DayOfWeek == DayOfWeek.Sunday;
        }

        return month;
    }
    
    [Fact]
    public void MapMonthWorkDayFormItemToMonthDayItem_WithValidData_IsValid()
    {
        
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        
        var mapper = new Mapper(config);

        var monthWorkDayFormItem = new WorkDayViewModel
        {
            Date = DateTimeOffset.Now,
            StartHour = TimeSpan.FromHours(11),
            EndHour = TimeSpan.FromHours(18),
            IsVacation = true
        };
        
        var monthWorkDayItem = mapper.Map<MonthWorkDayItem>(monthWorkDayFormItem);
        
        Assert.Equal(monthWorkDayFormItem.Date, monthWorkDayItem.Date);
        Assert.Equal(monthWorkDayFormItem.StartHour, monthWorkDayItem.StartHour);
        Assert.Equal(monthWorkDayFormItem.EndHour, monthWorkDayItem.EndHour);
        Assert.Equal(monthWorkDayFormItem.IsVacation, monthWorkDayItem.IsVacation);
    }
    
    [Fact]
    public void MapMonthWorkDayItemToMonthDayFormItem_WithValidData_IsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        
        var mapper = new Mapper(config);

        var monthWorkDayFormItem = new MonthWorkDayItem
        {
            Date = DateTimeOffset.Now,
            StartHour = TimeSpan.FromHours(11),
            EndHour = TimeSpan.FromHours(18),
            IsVacation = true
        };
        
        var monthWorkDayItem = mapper.Map<WorkDayViewModel>(monthWorkDayFormItem);
        
        Assert.Equal(monthWorkDayFormItem.Date, monthWorkDayItem.Date);
        Assert.Equal(monthWorkDayFormItem.StartHour, monthWorkDayItem.StartHour);
        Assert.Equal(monthWorkDayFormItem.EndHour, monthWorkDayItem.EndHour);
        Assert.Equal(monthWorkDayFormItem.IsVacation, monthWorkDayItem.IsVacation);
    }
    
    [Fact]
    public void MapListMonthWorkDayFormItemToMonthDayItem_WithValidData_IsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        
        var mapper = new Mapper(config);

        var monthWorkDayFormItemsList = BuildMonth();
        
        var monthWorkDayItem = mapper.Map<List<MonthWorkDayItem>>(monthWorkDayFormItemsList);
        
        Assert.Equal(monthWorkDayFormItemsList.First().Date, monthWorkDayItem.First().Date);
        Assert.Equal(monthWorkDayFormItemsList.First().StartHour, monthWorkDayItem.First().StartHour);
        Assert.Equal(monthWorkDayFormItemsList.First().EndHour, monthWorkDayItem.First().EndHour);
        Assert.Equal(monthWorkDayFormItemsList.First().IsVacation, monthWorkDayItem.First().IsVacation);
    }
    
    [Fact]
    public void MapListMonthWorkDayItemToMonthDayFormItem_WithValidData_IsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        
        var mapper = new Mapper(config);

        var monthWorkDayFormItemsList = new List<MonthWorkDayItem>()
        {
            new ()
            {
                Date = DateTimeOffset.Now,
                StartHour = TimeSpan.FromHours(11),
                EndHour = TimeSpan.FromHours(18),
                IsVacation = true
            }
        };
        
        var monthWorkDayItem = mapper.Map<List<WorkDayViewModel>>(monthWorkDayFormItemsList);
        
        Assert.Equal(monthWorkDayItem.First().Date, monthWorkDayFormItemsList.First().Date);
        Assert.Equal(monthWorkDayItem.First().StartHour, monthWorkDayFormItemsList.First().StartHour);
        Assert.Equal(monthWorkDayItem.First().EndHour, monthWorkDayFormItemsList.First().EndHour);
        Assert.Equal(monthWorkDayItem.First().IsVacation, monthWorkDayFormItemsList.First().IsVacation);
    }
    
    [Fact]
    public void MapMonthWorkDayFormListItemToMonthDayItem_WithValidData_IsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        
        var mapper = new Mapper(config);

        var days = BuildMonth();
        
        var month = new MonthWorkDay
        {
            DateMonth = DateTimeOffset.Now,
            EmployeeId = Guid.NewGuid(),
            Days = mapper.Map<List<MonthWorkDayItem>>(days),
        };
        
        Assert.Equal(month.Days.First().Date, days.First().Date);
        Assert.Equal(month.Days.First().StartHour, days.First().StartHour);
        Assert.Equal(month.Days.First().EndHour, days.First().EndHour);
        Assert.Equal(month.Days.First().IsVacation, days.First().IsVacation);
    }
}
