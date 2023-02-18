using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Terminal.ApiProducer.Services;

public class MessageProducer : IMessageProducer
{
    public void SendingMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName= "localhost",
            UserName= "user",
            Password= "mypass",
            VirtualHost= "/",
        };
        //factory.Uri =  "amqp://user:mypass@hostName:port/";
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare("booking" , 
                            durable:true,
                            exclusive : true);

        var jsonStr = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonStr);
         channel.BasicPublish(string.Empty,
                            "booking",
                            body:body);
        //connection.Close();
    }
}
