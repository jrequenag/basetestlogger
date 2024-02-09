using Microsoft.Azure.ServiceBus;

using System.Text;

namespace SerilogTestLog;

public interface IServiceBus {
    Task SendMessageAsync(string id);

}

public class ServiceBus : IServiceBus {
    private readonly IConfiguration _configuration;
    private readonly ILogger<ServiceBus> logger;
    private readonly IQueueClient _client;
    public ServiceBus(IConfiguration configuration, ILogger<ServiceBus> logger) {
        _configuration = configuration;
        this.logger = logger;
        _client = new QueueClient(_configuration["ServiceBus:ConnectionString"], _configuration["ServiceBus:QueueName"]);
    }

    public async Task SendMessageAsync(string id) {

        string messageBody = ($"Id: {id}");

        Message message = new(Encoding.UTF8.GetBytes(messageBody)) {
            MessageId = Guid.NewGuid().ToString(),
            ContentType = "application/json"
        };

        await _client.SendAsync(message);
        logger.LogInformation("Message with id {id} has be published ", message.MessageId);
    }
}