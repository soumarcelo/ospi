using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.AuthCredentials;
using Engine.Application.UseCases.Credentials;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api")]
public class VerifyEmailController(
    VerifyEmailCredentialUseCase verifyEmailUseCase,
    Logger<VerifyEmailController> logger) : ControllerBase
{
    [HttpPost("verify-email")]
    public async Task<ActionResult> VerifyEmail([FromBody] VerifyEmailCredentialDTO body)
    {
        EmailAddress email = new(body.Email);

        IResult<AuthCredential> result = await verifyEmailUseCase.Execute(email);
        if (!result.TryGetValue(out AuthCredential _, out Error? error))
        {
            logger.LogError("Email verification failed for {Email}: {ErrorMessage}", email, error?.Message);
            return BadRequest(error?.Message ?? "An unknown error occurred.");
        }

        logger.LogInformation("Email verification successful for {Email}.", email);
        return Ok($"Email {email} verified successfully.");
    }
}
