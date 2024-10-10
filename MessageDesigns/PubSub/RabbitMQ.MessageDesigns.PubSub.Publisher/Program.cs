using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string exchangeName = "example-pub-sub-exchange";

channel.ExchangeDeclare(
    exchange: exchangeName,
    type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(500);

    byte[] bytemessage = Encoding.UTF8.GetBytes($"Pub/Sub Message {i}");
    channel.BasicPublish(
        exchange: exchangeName,
        routingKey: String.Empty,
        body: bytemessage);
}

/* 
Bu tasarımda publisher mesajı bir exchange’e gönderir ve böylece mesaj bu exchange’e bind edilmiş olan tüm kuyruklara yönlendirilir. Bu tasarım, bir mesajın birçok tüketici tarafından işlenmesi gerektiği durumlarda kullanılır.
Publish/Subscribe tasarımı gerektiren senaryolarda genellikle Fanout Exchange kullanılmaktadır.
💡Bu tasarımda `BasicQos` metodu üzerinden bir ölçeklendirme ayarı yapılabilir.
 */