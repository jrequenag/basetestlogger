namespace ConsumerLoggerTest;
public interface ISubscriptionReceiver {
    Task ProcessMessagesAsync();
    Task StopProcessingAsync();
}