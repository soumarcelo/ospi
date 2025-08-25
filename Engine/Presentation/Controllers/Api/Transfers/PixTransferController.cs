using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.Transactions.Pix;
using Engine.Application.UseCases.Transactions.Pix;
using Engine.Presentation.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Presentation.Controllers.Api.Transfers;

[ApiController]
[Route("api/v1/transfers")]
[Authorize]
[ResultFilter]
public class PixTransferController(
    DebitInternalPixTransactionUseCase debitPixUseCase,
    ILogger<PixTransferController> logger) : ControllerBase
{
    [HttpPost("pix")]
    [PermissionAuthorization("PaymentAccountBalance.Read")]
    public async Task<IResult<Guid>> DebitPix([FromBody] CreatePixTransactionDTO dto)
    {
        IResult<Guid> result = await debitPixUseCase.Execute(dto);
        if (result.IsSuccess)
        {
            logger.LogInformation("Pix transfer successful: {@Result}", result.Value);
        }
        else
        {
            Error? error = result.Error;
            logger.LogWarning("Pix transfer failed: {ErrorMessage}", error?.Message);
        }
        return result;
    }
}
