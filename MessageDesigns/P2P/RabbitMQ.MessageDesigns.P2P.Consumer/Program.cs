using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string queueName = "example-p2p-queue";

channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: false,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();