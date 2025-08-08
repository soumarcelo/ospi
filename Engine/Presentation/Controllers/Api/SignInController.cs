using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.AuthCredentials;
using Engine.Application.DTOs.Users;
using Engine.Application.Requests;
using Engine.Application.UseCases.Users;
using Engine.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api/v1")]
[ResultFilter]
public class SignInController(SignInUseCase signInUseCase, Logger<SignInController> logger) : ControllerBase
{
    [HttpPost("signin")]
    public async Task<IResult<AuthTokenDTO>> SignIn([FromBody] SignInDTO body)
    {
        SignInRequest request = SignInRequest.FromDTO(body);
        IResult<AuthTokenDTO> result = await signInUseCase.Execute(request);
        if (result.IsSuccess)
        {
            logger.LogInformation("Sign-in successful for email: {Email}", request.Email);
        }
        else
        {
            Error? error = result.Error;
            logger.LogError(
                "Sign-in request failed for email: {Email}. Error: {ErrorMessage}",
                request.Email,
                error?.Message);
        }

        return result;
    }
}
