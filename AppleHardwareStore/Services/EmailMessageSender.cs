using System.Runtime.Serialization;

namespace AppleHardwareStore.Services
{ 
    public class EmailMessageSender : IMessageSender
    {
        public string Send(string message)
        {
            return $"Sent by Email {message}";
        }
    }
}