using Demo.Bank.Accounts.Api.Infrastructure.Messaging;

namespace Demo.Bank.Accounts.Api.Shared;

public class ErrorCodes
{
    public const string InvalidRequest = nameof(InvalidRequest);
    public const string InternalServerError = nameof(InternalServerError);
    public const string InvalidMessage = nameof(InvalidMessage);
    public const string CannotCreateQueue = nameof(CannotCreateQueue);
    public const string MessagePublishError = nameof(MessagePublisher);
}

public class ErrorMessages
{
    public const string InvalidRequest = "invalid request";
    public const string InternalServerError = "internal server error occurred";
    public const string InvalidMessage = "invalid message";
    public const string CannotCreateQueue = "error occurred when creating the queue";
    public const string MessagePublishError = "error occurred when publishing a message to the queue";
}