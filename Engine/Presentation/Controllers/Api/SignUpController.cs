using Engine.Application.Common.Results;
using Engine.Application.DTOs.Users;
using Engine.Application.Requests;
using Engine.Application.UseCases.Users;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api")]
public class SignUpController(
    ILogger<SignUpController> logger,
    SignUpUseCase signUpUseCase) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult> SignUp([FromBody] SignUpDTO body)
    {
        SignUpRequest request = SignUpRequest.FromDTO(body);

        IResult<Guid> result = await signUpUseCase.Execute(request);

        if (!result.IsSuccess)
        {
            logger.LogError(
                "Sign-up request failed for email: {Email}. Error: {ErrorMessage}", 
                request.Email,
                result.ErrorMessage);
            return BadRequest(result.ErrorMessage);
        }

        logger.LogInformation("Sign-up request successful for email: {Email}", request.Email);
        return Ok("Sign-up successful.");
    }
}
