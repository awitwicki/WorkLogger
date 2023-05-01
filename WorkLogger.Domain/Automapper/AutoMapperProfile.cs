using AutoMapper;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Domain.Automapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MonthWorkDayItem, MonthDayFormItem>();
        CreateMap<MonthDayFormItem, MonthWorkDayItem>();
    }
}
