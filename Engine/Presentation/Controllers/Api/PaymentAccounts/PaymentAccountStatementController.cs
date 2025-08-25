using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.Transactions;
using Engine.Application.UseCases.Transactions;
using Engine.Presentation.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api.PaymentAccounts;

[ApiController]
[Route("api/v1/payment-accounts")]
[Authorize]
[ResultFilter]
public class PaymentAccountStatementController(
    GetPaymentAccountStatementUseCase statementUseCase,
    ILogger<PaymentAccountStatementController> logger) : ControllerBase
{
    [HttpGet("{accountId}/statement")]
    [PermissionAuthorization("PaymentAccountStatement.Read")]
    public async Task<IResult<IList<StatementTransactionDTO>>> GetStatement(
        [FromRoute] Guid accountId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        IResult<IList<StatementTransactionDTO>> result = await statementUseCase.Execute(accountId);
        if (result.IsSuccess)
        {
            logger.LogInformation(
                "Retrieved statement for account {AccountId} with {TransactionCount} transactions.",
                accountId,
                result.Value?.Count ?? 0);
        }
        else
        {
            Error? error = result.Error;
            logger.LogError(
                "Failed to retrieve statement for account {AccountId}: {ErrorMessage}",
                accountId,
                error?.Message);
        }

        return result;
    }
}
