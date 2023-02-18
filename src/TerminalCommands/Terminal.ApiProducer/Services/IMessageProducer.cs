namespace Terminal.ApiProducer.Services;

public interface IMessageProducer
{
    void SendingMessage<T>(T message);
}
