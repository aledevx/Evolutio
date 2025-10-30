using AutoMapper;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Enums;
using Sqids;

namespace Evolutio.Application.Services.AutoMapper;
public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;
    public AutoMapping(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;
        RequestToDomain();
        DomainToResponse();
    }
    // Requests -> Domínio
    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
        .ForMember(dest => dest.Password, opt => opt.Ignore())
        .ForMember(dest => dest.Perfil, opt => opt.MapFrom(source => Enum.Parse<Perfil>(source.Perfil.ToString())));

        CreateMap<RequestUpdateUserJson, Domain.Entities.User>();
    }
    // Domínio -> Responses
    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
    }
}

