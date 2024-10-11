using MassTransit;
using RabbitMQ.MassTransit.Console.Consumer.Consumers;

string rabbitMQUri = "amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi";

string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);

    factory.ReceiveEndpoint(queueName, enpoint =>
    {
        enpoint.Consumer<MessageConsumer>();
    });
});

bus.StartAsync();

Console.Read();