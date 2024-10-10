using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string queueName = "example-work-queue";

channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(500);

    byte[] bytemessage = Encoding.UTF8.GetBytes($"Work Queue Message {i}");
    channel.BasicPublish(
        exchange: String.Empty,
        routingKey: queueName,
        body: bytemessage);
}

/* 
Bu tasarımda, publisher tarafından yayınlanmış bir mesajın birden fazla consumer arasından yalnızca biri tarafından tüketilmesi amaçlanmaktadır. Böylece mesajların işlenmesi sürecinde tüm consumer’lar aynı iş yüküne ve eşit görev dağılımına sahip olacaktırlar.
Work Queue tasarımı gerektiren senaryolarda genellikle Direct Exchange kullanılmaktadır.
💡Tüm consumer’ların aynı iş yüküne ve eşit görev dağılımına sahip olabilmeleri için `BasicQos` metodu ile ölçeklendirme çalışması yapılmalı. 
*/