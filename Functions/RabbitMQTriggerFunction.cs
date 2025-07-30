
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;
using FastTechFoodsDLQProcessor.Services;

namespace FastTechFoodsDLQProcessor.Functions;

/// <summary>
/// RabbitMQ trigger function for processing DLQ messages directly from the queue
/// </summary>
public class RabbitMQTriggerFunction
{
    private readonly ILogger<RabbitMQTriggerFunction> _logger;
    private readonly Services.DLQMessageService _dlqMessageService;

    public RabbitMQTriggerFunction(ILogger<RabbitMQTriggerFunction> logger, Services.DLQMessageService dlqMessageService)
    {
        _logger = logger;
        _dlqMessageService = dlqMessageService;
    }

    /// <summary>
    /// RabbitMQ trigger that processes messages directly from order.dlq.queue
    /// </summary>
    [Function("ProcessDLQMessage")]
    public async Task ProcessDLQMessageAsync(
        [RabbitMQTrigger("order.dlq.queue", ConnectionStringSetting = "RabbitMQConnectionString")] byte[] messageBody)
    {
        try
        {
            var message = Encoding.UTF8.GetString(messageBody);
            _logger.LogInformation("Received DLQ message: {Message}", message);
            
            // Process the message here
            await ProcessMessageAsync(message);

            // Save message to MongoDB
            await _dlqMessageService.SaveMessageAsync(message);
            
            _logger.LogInformation("Successfully processed DLQ message");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing DLQ message");
            throw; // This will cause the message to be requeued or sent to dead letter
        }
    }

    private async Task ProcessMessageAsync(string message)
    {
        // Add your message processing logic here
        _logger.LogInformation("Processing message content: {MessageContent}", message);
        
        // Example: Parse JSON, validate, transform, or forward to another queue
        // For now, just log the message
        await Task.Delay(100); // Simulate some processing work
        
        _logger.LogInformation("Message processing completed");
    }
}
