using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Mesaj: ");
    string message = Console.ReadLine();
    byte[] bytemessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "direct-exchange-example", routingKey: "direct-queue-example", body: bytemessage);
}
