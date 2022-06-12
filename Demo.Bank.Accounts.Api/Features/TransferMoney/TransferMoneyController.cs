using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using Demo.Bank.Accounts.Api.Features.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;

namespace Demo.Bank.Accounts.Api.Features.TransferMoney;

[ApiController]
public class TransferMoneyController : ControllerBase
{
    private readonly ITransferMoneyService _service;
    private readonly IResponseCreator<TransferMoneyRequest> _responseCreator;

    public TransferMoneyController(ITransferMoneyService service, IResponseCreator<TransferMoneyRequest> responseCreator)
    {
        _service = service;
        _responseCreator = responseCreator;
    }
    
    [HttpPost("api/accounts/transfer")]
    [ProducesResponseType(typeof(EmptyResult), (int) HttpStatusCode.Accepted, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> TransferAsync(
        [Required][FromHeader(Name = ApplicationHeaders.CorrelationId)] string correlationId,
        [FromBody] TransferMoneyDto dto)
    {
        var request = GetRequest(dto);
        request.CorrelationId = correlationId;

        var operation = await _service.TransferMoneyAsync(request);

        return _responseCreator.CreateResponse(request, operation);
    }
    
    private TransferMoneyRequest GetRequest(TransferMoneyDto dto)
    {
        var request = new TransferMoneyRequest
        {
            CorrelationId = dto?.CorrelationId ?? string.Empty,
            CustomerId = dto?.CustomerId ?? string.Empty,
            FromAccount = dto?.FromAccount ?? string.Empty,
            ToAccount = dto?.ToAccount?? string.Empty,
            Amount = dto?.Amount?? 0,
            TransferredAt = DateTime.UtcNow
        };

        return request;
    }
}