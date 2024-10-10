using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string requestQueueName = "example-request-response-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: requestQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

    byte[] bytemessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı: {message}");

    IBasicProperties basicProperties = channel.CreateBasicProperties();
    basicProperties.CorrelationId = e.BasicProperties.CorrelationId;

    channel.BasicPublish(
        exchange: String.Empty,
        routingKey: e.BasicProperties.ReplyTo,
        basicProperties: basicProperties,
        body:  bytemessage);
};

Console.Read();