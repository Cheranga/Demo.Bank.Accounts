using System.Net;
using Demo.Bank.Accounts.Api.Features.Shared;
using Demo.Bank.Accounts.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Bank.Accounts.Api.Features.TransferMoney;

public class TransferMoneyResponseCreator : IResponseCreator<TransferMoneyRequest>
{
    public IActionResult CreateResponse(TransferMoneyRequest request, Result operation)
    {
        if (operation.Status)
        {
            return new AcceptedResult();
        }

        return GetErrorMessage(operation);
    }

    private IActionResult GetErrorMessage(Result operation)
    {
        ErrorResponse errorResponse;
        switch (operation.ErrorCode)
        {
            case ErrorCodes.InvalidRequest:
                errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.InvalidRequest,
                    ErrorMessage = ErrorMessages.InvalidRequest,
                    Errors = operation.ValidationResult.Errors.Select(x => new ErrorData
                    {
                        ErrorCode = x.PropertyName,
                        ErrorMessage = x.ErrorMessage
                    }).ToList()
                };

                return new BadRequestObjectResult(errorResponse);

            default:
                errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorMessages.InternalServerError,
                    Errors = operation.ValidationResult.Errors.Select(x => new ErrorData
                    {
                        ErrorCode = x.ErrorCode,
                        ErrorMessage = x.ErrorMessage
                    }).ToList()
                };

                return new ObjectResult(errorResponse)
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError
                };
        }
    }
}