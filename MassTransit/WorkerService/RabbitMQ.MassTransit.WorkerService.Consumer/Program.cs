using MassTransit;
using RabbitMQ.MassTransit.WorkerService.Consumer;
using RabbitMQ.MassTransit.WorkerService.Consumer.Consumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<MessageConsumer>();

    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host("amqps://aczdvobi:WJs-zmyj6qwBqf4FAMoFzrZfQ2wEwt0t@toad.rmq.cloudamqp.com/aczdvobi");

        _configurator.ReceiveEndpoint("example-message-queue", e => e.ConfigureConsumer<MessageConsumer>(context));
    });
});

var host = builder.Build();
await host.RunAsync();
