using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.UseCases.Transactions;
using Engine.Presentation.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api;

[ApiController]
[Route("api/v1")]
[Authorize]
[ResultFilter]
public class PaymentAccountBalanceController(
    GetPaymentAccountBalanceUseCase balanceUseCase,
    ILogger<PaymentAccountBalanceController> logger) : ControllerBase
{
    [HttpGet("payment-accounts/{accountId}/balance")]
    [PermissionAuthorization("PaymentAccountBalance.Read")]
    public async Task<IResult<decimal>> GetBalance([FromRoute] Guid accountId)
    {
        IResult<decimal> result = await balanceUseCase.Execute(accountId);
        if (result.IsSuccess)
        {
            logger.LogInformation(
                "Retrieved balance for account {AccountId}: {Balance}", accountId, result.Value);
        }
        else
        {
            Error? error = result.Error;
            logger.LogError(
                "Failed to retrieve balance for account {AccountId}: {ErrorMessage}", 
                accountId, error?.Message);
        }

        return result;
    }
}
