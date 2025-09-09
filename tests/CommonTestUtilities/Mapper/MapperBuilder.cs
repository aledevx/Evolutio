using AutoMapper;
using CommonTestUtilities.IdEncryption;
using Evolutio.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;
public class MapperBuilder
{
    public static IMapper Build() 
    {
        var idEncripter = IdEncripterBuilder.Build();

        var mapperConfig = new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping(idEncripter));
        }).CreateMapper();

        return mapperConfig;
    }
}

