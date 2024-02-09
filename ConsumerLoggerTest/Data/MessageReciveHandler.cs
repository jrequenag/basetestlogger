namespace ConsumerLoggerTest.Data;
public class MessageReciveHandler {
    private readonly ILogger<MessageReciveHandler> _logger;
    private readonly ApplicationContext _applicationContext;

    public MessageReciveHandler(ILoggerFactory loggerFactory, ApplicationContext applicationContext) {
        _logger = loggerFactory.CreateLogger<MessageReciveHandler>();
        _applicationContext = applicationContext;
    }
    public async Task<string> Handler(string topic) {
        _logger.LogInformation("Message was be received {topic}", topic);
        int id = ExtractId(topic);
        if (id == 0)
            throw new Exception("Id cant no be 0");
        Customer? customer = await _applicationContext.Customer.FindAsync(id);
        if (customer is null)
            throw new Exception("Customer not found");
        _logger.LogInformation("Customer found {customer}", customer);
        return "Message was been processed";
    }
    private int ExtractId(string topicBody) {
        string[] fields = topicBody.Split(',');
        foreach (string field in fields) {
            string[] keyValue = field.Split(':');
            if (keyValue[0].Trim() == "Id")
                return int.Parse(keyValue[1].Trim());
        }
        throw new ArgumentException("Id not found in topic body");
    }
}
