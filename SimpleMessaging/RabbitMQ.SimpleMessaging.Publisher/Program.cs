using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.QueueDeclare(queue: "example-queue", exclusive: false);

byte[] bytemessage = Encoding.UTF8.GetBytes("SimpleMessaging");
channel.BasicPublish(exchange: "", routingKey: "example-queue", body: bytemessage);

Console.Read();