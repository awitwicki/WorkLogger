using AutoMapper;
using WorkLogger.Domain.Automapper;

namespace WorkLogger.Domain.Tests.AutomapperProfileTests;

public class AutomapperProfileTests
{
    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        config.AssertConfigurationIsValid();
    }
}
