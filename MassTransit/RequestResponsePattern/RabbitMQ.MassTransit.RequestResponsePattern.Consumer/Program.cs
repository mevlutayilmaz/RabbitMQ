using MassTransit;
using RabbitMQ.MassTransit.RequestResponsePattern.Consumer.Consumers;

Console.WriteLine("Consumer");

string rabbitMQUri = "amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi";

string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);

    factory.ReceiveEndpoint(requestQueue, enpoint =>
    {
        enpoint.Consumer<RequestMessageConsumer>();
    });
});

await bus.StartAsync();

Console.Read();