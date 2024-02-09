using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace ConsumerLoggerTest.Data;
internal class SubscriptionReceiver : ISubscriptionReceiver {
    private readonly ILogger<SubscriptionReceiver> _logger;
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusProcessor _processor;
    private readonly ServiceBusAdministrationClient _clientAdministration;
    private readonly string _queuename;
    private readonly ILoggerFactory _loggerfactory;
    private readonly ApplicationContext _applicationContext;

    public SubscriptionReceiver(IConfiguration configuration,
                                ILoggerFactory loggerfactory,
                                ApplicationContext applicationContext) {
        _clientAdministration = new ServiceBusAdministrationClient((configuration["ServiceBus:ConnectionString"]));
        _serviceBusClient = new ServiceBusClient(configuration["ServiceBus:ConnectionString"]);
        _queuename = configuration["ServiceBus:QueueName"] ?? throw new ArgumentNullException(configuration["ServiceBus:QueueName"], "The QueueName in configuration it's required");
        _processor = _serviceBusClient.CreateProcessor(_queuename, new ServiceBusProcessorOptions());
        _logger = loggerfactory.CreateLogger<SubscriptionReceiver>();
        _loggerfactory = loggerfactory;
        this._applicationContext = applicationContext;
    }
    async Task MessageHandler(ProcessMessageEventArgs args) {
        string body = args.Message.Body.ToString();
        _logger.LogInformation($"[Test] Received: {body} from subscription: S1");
        MessageReciveHandler handler = new(_loggerfactory, _applicationContext);
        _logger.LogInformation(await handler.Handler(body));
        await args.CompleteMessageAsync(args.Message);
    }

    Task ErrorHandler(ProcessErrorEventArgs args) {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    public async Task ProcessMessagesAsync() {
        await Configure();
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;
        await _processor.StartProcessingAsync();
    }
    private async Task Configure() {
        if (!(await _clientAdministration.QueueExistsAsync(_queuename))) {
            CreateQueueOptions options = new(_queuename) {
                DeadLetteringOnMessageExpiration = true,
                DefaultMessageTimeToLive = TimeSpan.FromMinutes(10),
                LockDuration = TimeSpan.FromSeconds(60),
                MaxDeliveryCount = 3
            };
            await _clientAdministration.CreateQueueAsync(options);
            _logger.LogInformation("The Queue {queuename}, has been created", _queuename);
        }
    }

    public async Task StopProcessingAsync() {
        await _processor.StopProcessingAsync();
        await _processor.DisposeAsync();
        await _serviceBusClient.DisposeAsync();
    }
}
