using System.Text.Json.Serialization;
using Azure.Storage.Queues;
using Demo.Bank.Accounts.Api.Shared;
using Newtonsoft.Json;

namespace Demo.Bank.Accounts.Api.Infrastructure.Messaging;

public interface IMessagePublisher
{
    Task<Result> PublishAsync<TData>(string queue, TData data) where TData : IIdentifier;
}

public class MessagePublisher : IMessagePublisher
{
    private readonly QueueServiceClient _queueServiceClient;
    private readonly ILogger<MessagePublisher> _logger;
    private readonly JsonSerializerSettings _serializerSettings;

    public MessagePublisher(QueueServiceClient queueServiceClient, ILogger<MessagePublisher> logger)
    {
        _queueServiceClient = queueServiceClient;
        _serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            Error = (_, args) => args.ErrorContext.Handled = true
        };
        _logger = logger;
    }
    
    public async Task<Result> PublishAsync<TData>(string queue, TData data) where TData : IIdentifier
    {
        if (string.IsNullOrEmpty(queue))
        {
            return Result.Failure(ErrorCodes.InvalidMessage, ErrorMessages.InvalidMessage);
        }

        var queueClient = _queueServiceClient.GetQueueClient(queue);
        await queueClient.CreateIfNotExistsAsync();
        
        var messageContent = JsonConvert.SerializeObject(data, _serializerSettings);
        var sendOperation = await queueClient.SendMessageAsync(messageContent);
        
        if (sendOperation.GetRawResponse().IsError)
        {
            _logger.LogError("{CorrelationId} error occurred when publishing message {Message}", data.CorrelationId, messageContent);
            return Result.Failure(ErrorCodes.MessagePublishError, ErrorMessages.MessagePublishError);
        }

        _logger.LogInformation("{CorrelationId} message published successfully", data.CorrelationId);
        return Result.Success();
    }
}

