using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api")]
public class PaymentAccountStatementController(
    ILogger<PaymentAccountStatementController> logger) : ControllerBase
{
}
