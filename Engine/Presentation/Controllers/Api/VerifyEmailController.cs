using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.AuthCredentials;
using Engine.Application.UseCases.Credentials;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;
using Engine.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api/v1")]
[ResultFilter]
public class VerifyEmailController(
    VerifyEmailCredentialUseCase verifyEmailUseCase,
    Logger<VerifyEmailController> logger) : ControllerBase
{
    [HttpPost("verify-email")]
    public async Task<IResult<AuthCredential>> VerifyEmail([FromBody] VerifyEmailCredentialDTO body)
    {
        EmailAddress email = new(body.Email);
        IResult<AuthCredential> result = await verifyEmailUseCase.Execute(email);
        if (result.IsSuccess)
        {
            logger.LogInformation("Email verification initiated for {Email}.", email);
        }
        else
        {
            Error? error = result.Error;
            logger.LogError("Email verification failed for {Email}: {ErrorMessage}", email, error?.Message);
        }

        return result;
    }
}
