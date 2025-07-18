using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Services.Tests.Helpers;

public class MapperFactory
{
    public static IMapper Create<TProfile>() where TProfile : Profile, new()
    {
        var configExpression = new MapperConfigurationExpression();
        configExpression.AddProfile<TProfile>();

        var config = new MapperConfiguration(configExpression, new LoggerFactory());
        return config.CreateMapper();
    }
}