using Demo.Bank.Accounts.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Bank.Accounts.Api.Features.Shared;

public interface IResponseCreator<in TRequest>
{
    IActionResult CreateResponse(TRequest request, Result operation);
}