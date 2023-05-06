using AutoMapper;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Domain.Automapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MonthWorkDayItem, WorkDayViewModel>()
            .ForMember(x => x.Holiday, opt => opt.Ignore());
        CreateMap<WorkDayViewModel, MonthWorkDayItem>();
        CreateMap<MonthWorkDay, UserWorkMonthViewModel>();
    }
}
