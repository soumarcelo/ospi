using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IResult = Engine.Application.Common.Results.IResult;

namespace Engine.Presentation.Filters;

public class ResultFilterAttribute : ActionFilterAttribute
{
    public new int Order => 10;

    private static void ProcessError(IResult result, ResultExecutingContext context)
    {
        Error? error = result.Error;
        int statusCode = error?.ErrorType switch
        {
            ErrorType.NotFound => 404, // Not Found
            ErrorType.ValidationError => 422, // Unprocessable Entity
            ErrorType.BadRequest => 400, // Bad Request
            ErrorType.Unauthorized => 401, // Unauthorized
            ErrorType.Forbidden => 403, // Forbidden
            ErrorType.Conflict => 409, // Conflict
            _ => 500, // Default para outros erros
        };

        context.Result =  new ObjectResult(error?.Message)
        {
            StatusCode = statusCode
        };
    }

    private static void ProcessSuccess(IResult result, ResultExecutingContext context)
    {
        if (result is IResult<object> resultWithGenericValue)
        {
            context.Result = new OkObjectResult(resultWithGenericValue.Value);
        }
        else
        {
            context.Result = new OkResult();
        }
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is IResult result)
        {
            if (result.IsSuccess)
            {
                ProcessSuccess(result, context);
                return;
            }

            ProcessError(result, context);
        }
    }
}
