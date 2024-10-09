using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "fanout-exchange-example", 
    type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(1000);
    byte[] bytemessage = Encoding.UTF8.GetBytes($"Fanout Exchange {i}");

    channel.BasicPublish(
        exchange: "fanout-exchange-example", 
        routingKey: String.Empty, 
        body: bytemessage);
}