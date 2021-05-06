using RabbitMQ.Client;
using System.Text;

namespace BookCatalogue.Microservice.Messages
{
    public class MessageSender
    {
       
        public void SendMQ(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "demoqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: "demoqueue", basicProperties: null, body: body);
            }
        }
    }
}
