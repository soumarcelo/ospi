using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api")]
public class PaymentAccountBalanceController(
    ILogger<PaymentAccountBalanceController> logger) : ControllerBase
{
}
