namespace Demo.Bank.Accounts.Api.Shared;

public class ErrorCodes
{
    public const string InvalidRequest = nameof(InvalidRequest);
    public const string InternalServerError = nameof(InternalServerError);
}

public class ErrorMessages
{
    public const string InvalidRequest = "invalid request";
    public const string InternalServerError = "internal server error occurred";
}