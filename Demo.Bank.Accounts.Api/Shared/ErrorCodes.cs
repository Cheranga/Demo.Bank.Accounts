using Demo.Bank.Accounts.Api.Infrastructure.Messaging;

namespace Demo.Bank.Accounts.Api.Shared;

public class ErrorCodes
{
    public const string InvalidRequest = nameof(InvalidRequest);
    public const string InternalServerError = nameof(InternalServerError);
    public const string InvalidMessage = nameof(InvalidMessage);
    public const string CannotCreateQueue = nameof(CannotCreateQueue);
    public const string MessagePublishError = nameof(MessagePublisher);
    public const string InvalidCommand = nameof(InvalidCommand);
    public const string CommandError = nameof(CommandError);
    public const string QueueDoesNotExist = nameof(QueueDoesNotExist);
    public const string MessageReadError = nameof(MessageReadError);
}

public class ErrorMessages
{
    public const string InvalidRequest = "invalid request";
    public const string InternalServerError = "internal server error occurred";
    public const string InvalidMessage = "invalid message";
    public const string CannotCreateQueue = "error occurred when creating the queue";
    public const string MessagePublishError = "error occurred when publishing a message to the queue";
    public const string InvalidCommand = "invalid command";
    public const string CommandError = "error occurred when executing the data command";
    public const string QueueDoesNotExist = "queue does not exists";
    public const string MessageReadError = "error occurred when reading the message";
}