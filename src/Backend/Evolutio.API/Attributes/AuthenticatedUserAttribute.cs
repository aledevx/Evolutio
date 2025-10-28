using Evolutio.API.Filters;
using Evolutio.Communication.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute(params Perfil[] roles) : base(typeof(AuthenticadedUserFilter))
    {
        Arguments = new object[] { roles ?? Array.Empty<Perfil>() };
    }
}
