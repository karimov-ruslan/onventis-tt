namespace OnventisTT.Messaging.Abstraction
{
    public interface IMessageSender
    {
        void SendMessage<T>(T message);
    }
}
