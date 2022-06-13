using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using Demo.Bank.Accounts.Api.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Bank.Accounts.Api.Features.CreateBankAccount;

[ApiController]
public class CreateBankAccountController : ControllerBase
{
    private readonly IResponseCreator<CreateBankAccountRequest> _responseCreator;
    private readonly ICreateBankAccountService _service;

    public CreateBankAccountController(ICreateBankAccountService service,
        IResponseCreator<CreateBankAccountRequest> responseCreator)
    {
        _service = service;
        _responseCreator = responseCreator;
    }

    [HttpPost("api/accounts")]
    [ProducesResponseType(typeof(EmptyResult), (int) HttpStatusCode.Accepted, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> CreateAccountAsync(
        [Required] [FromHeader(Name = ApplicationHeaders.CorrelationId)]
        string correlationId,
        [Required] [FromBody] CreateBankAccountDto dto)
    {
        var request = GetRequest(dto);
        request.CorrelationId = correlationId;

        var operation = await _service.CreateAccountAsync(request);

        return _responseCreator.CreateResponse(request, operation);
    }

    private CreateBankAccountRequest GetRequest(CreateBankAccountDto dto)
    {
        var request = new CreateBankAccountRequest
        {
            BankAccountId = Guid.NewGuid().ToString("N").ToUpper(),
            CorrelationId = dto?.CorrelationId ?? string.Empty,
            ClientId = dto?.ClientId?? string.Empty,
            Name = dto?.Name ?? string.Empty,
            Address = dto?.Address ?? string.Empty,
            OpeningBalance = dto?.OpeningBalance ?? 0
        };

        return request;
    }
}