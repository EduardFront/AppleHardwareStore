namespace AppleHardwareStore.Services
{
    public class SmsMessageSender : IMessageSender
    {
        public string Send(string message) 
        {
            return $"Sent by Sms {message}";
        }
    }
}