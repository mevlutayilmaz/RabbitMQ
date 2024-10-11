using MassTransit;
using RabbitMQ.MassTransit.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.MassTransit.Console.Consumer.Consumers
{
    public class MessageConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            System.Console.WriteLine($"Gelen mesaj: {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
