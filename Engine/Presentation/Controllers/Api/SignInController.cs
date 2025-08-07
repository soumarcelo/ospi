using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.Users;
using Engine.Application.Requests;
using Engine.Application.UseCases.Users;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api")]
public class SignInController(SignInUseCase signInUseCase, Logger<SignInController> logger) : ControllerBase
{
    [HttpPost("signin")]
    public async Task<ActionResult<SignInResponseDTO>> SignIn([FromBody] SignInDTO body)
    {
        SignInRequest request = SignInRequest.FromDTO(body);
        IResult<SignInResponseDTO> result = await signInUseCase.Execute(request);
        if (!result.TryGetValue(out SignInResponseDTO? response, out Error? error))
        {
            logger.LogError(
                "Sign-in request failed for email: {Email}. Error: {ErrorMessage}",
                request.Email,
                error?.Message);
            return BadRequest(error?.Message ?? "An unknown error occurred.");
        }

        logger.LogInformation("Sign-in successful for email: {Email}", request.Email);
        return Ok(response);
    }
}
