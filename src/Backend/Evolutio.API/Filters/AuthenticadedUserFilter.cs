using Evolutio.Communication.Enums;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Repositories.User;
using Evolutio.Domain.Security.Tokens;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Evolutio.API.Filters;
public class AuthenticadedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly IUserReadOnlyRepository _repository;
    private readonly Perfil[] _allowedRoles;
    public AuthenticadedUserFilter(IAccessTokenValidator accessTokenValidator,
        IUserReadOnlyRepository repository,
        Perfil[] allowedRoles)
    {
        _accessTokenValidator = accessTokenValidator;
        _repository = repository;
        _allowedRoles = allowedRoles ?? Array.Empty<Perfil>();
    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TakeOnRequest(context);
            (var identifier, var perfil) = _accessTokenValidator.ValidateAndGetUserIdentifierAndRole(token);

            var userExists = await _repository.ExistsByIdentifier(identifier);

            if (!userExists)
            {
                throw new NoPermissionException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }

            // Se foram informadas roles no atributo, checa se o perfil do token está entre elas.
            // Comparação caso-insensitive para evitar problemas de caixa.
            if (_allowedRoles.Length > 0)
            {
                var isInAllowedRole = _allowedRoles
                    .Any(r => r.Equals(Enum.Parse<Perfil>(perfil.ToString())));

                if (!isInAllowedRole)
                {
                    // Usuário autenticado, mas não tem a role necessária
                    throw new NoPermissionException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
                }
            }

        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("")
            {
                TokenIsExpired = true
            });
        }
        catch (EvolutioException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(
                new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE)
            );
        }
    }
    private static string TakeOnRequest(AuthorizationFilterContext context) 
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authentication)) 
        {
            throw new NoPermissionException(ResourceMessagesException.NO_TOKEN);
        }

        return authentication["Bearer ".Length..].Trim();
    }
}
