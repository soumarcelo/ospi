using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.Users;
using Engine.Application.Requests;
using Engine.Application.UseCases.Users;
using Engine.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api/v1")]
[ResultFilter]
public class SignUpController(
    ILogger<SignUpController> logger,
    SignUpUseCase signUpUseCase) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IResult<Guid>> SignUp([FromBody] SignUpDTO body)
    {
        SignUpRequest request = SignUpRequest.FromDTO(body);

        IResult<Guid> result = await signUpUseCase.Execute(request);
        if (result.IsSuccess)
        {
            logger.LogInformation("Sign-up request successful for email: {Email}", request.Email);
        }
        else
        {
            Error? error = result.Error;
            logger.LogError(
                "Sign-up request failed for email: {Email}. Error: {ErrorMessage}",
                request.Email,
                error?.Message);
        }
        
        return result;
    }
}
