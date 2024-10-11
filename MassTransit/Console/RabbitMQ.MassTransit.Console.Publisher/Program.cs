using MassTransit;
using RabbitMQ.MassTransit.Shared.Messages;

string rabbitMQUri = "amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi";

string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));

Console.Write("Göderilecek mesaj: ");
string message = Console.ReadLine();

await sendEndpoint.Send<IMessage>(new Message
{
    Text = message
});

Console.Read();

