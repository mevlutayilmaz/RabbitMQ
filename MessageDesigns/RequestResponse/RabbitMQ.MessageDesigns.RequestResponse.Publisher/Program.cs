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

string replyQueueName = channel.QueueDeclare().QueueName;
string correlationId = Guid.NewGuid().ToString();

IBasicProperties basicProperties = channel.CreateBasicProperties();
basicProperties.CorrelationId = correlationId;
basicProperties.ReplyTo = replyQueueName;

for (int i = 0; i < 10; i++)
{
    byte[] bytemessage = Encoding.UTF8.GetBytes($"Request Message {i}");
    channel.BasicPublish(
        exchange: String.Empty,
        routingKey: requestQueueName,
        basicProperties: basicProperties,
        body: bytemessage);
}

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    if(e.BasicProperties.CorrelationId == correlationId)
    {
        Console.WriteLine($"Reponse Message: {Encoding.UTF8.GetString(e.Body.Span)}");
    }
};

Console.Read();

/* 
Bu tasarımda, publisher bir request yapar bir kuyruğa mesaj gönderir ve bu mesajı tüketen consumer’dan sonuca dair başka kuyruktan bir yanıt response bekler. Bu tarz senaryolar için oldukça uygun bir tasarımıdır.
Bu tasarımda Publisher ve Consumer özünde hem publisher hem de consumer işlemlerini aynı anda yürütmektedirler. 
*/