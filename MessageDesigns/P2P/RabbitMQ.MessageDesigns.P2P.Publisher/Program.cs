using RabbitMQ.Client;
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

byte[] bytemessage = Encoding.UTF8.GetBytes("P2P Design");
channel.BasicPublish(
    exchange: String.Empty,
    routingKey: queueName,
    body: bytemessage);

/* 
Bu tasarımda, bir publisher ilgili mesajı direkt bir kuyruğa gönderir ve bu mesaj kuyruğu işleyen bir consumer tarafından tüketilir. Eğer ki senaryo gereği bir mesajın bir tüketici tarafından işlenmesi gerekiyorsa bu yaklaşım kullanılır.
P2P tasarımı gerektiren senaryolarda Direct Exchange kullanılmaktadır.
 */