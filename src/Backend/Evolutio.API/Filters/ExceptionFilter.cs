using Evolutio.Communication.Responses;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Evolutio.API.Filters;
public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is EvolutioException evolutioException)
        {
            HandleProjectException(evolutioException, context);
        }
        else
        {
            ThrowUnknownException(context);
        }
    }
    private static void HandleProjectException(EvolutioException evolutioException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)evolutioException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(evolutioException.GetErrorMessages()));
    }

    private static void ThrowUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
    }
}

