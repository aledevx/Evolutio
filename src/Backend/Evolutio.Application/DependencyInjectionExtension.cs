using Evolutio.Application.Services.AutoMapper;
using Evolutio.Application.UseCases.User.GetById;
using Evolutio.Application.UseCases.User.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Evolutio.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddUseCases(services);
        AddIdEncoder(services, configuration);
        AddAutoMapper(services);
    }
    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
    }
    private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
    {
        // Corrigido: Usar GetSection e .Value para obter o valor da configuração
        var alphabet = configuration.GetSection("Settings:IdCryptographyAlphabet").Value!;

        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = alphabet
        });

        services.AddSingleton(sqids);
    }
    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOptions =>
        {
            var sqids = option.GetService<SqidsEncoder<long>>()!;
            autoMapperOptions.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }
}

