using MassTransit;
using RabbitMQ.MassTransit.Shared.RequestResponseMessage;

Console.WriteLine("Publisher");

string rabbitMQUri = "amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi";

string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

await bus.StartAsync();

var request =  bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueue}"));

int i = 1;
while (true)
{
    await Task.Delay(200);
    var response = await request.GetResponse<ResponseMessage>(new()
    {
        MessageNo = i,
        Text = $"{i++}. request"
    });
    Console.WriteLine($"Response Received: {response.Message.Text}");
}