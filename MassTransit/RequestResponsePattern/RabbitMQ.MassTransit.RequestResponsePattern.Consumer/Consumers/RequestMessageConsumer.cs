using MassTransit;
using RabbitMQ.MassTransit.Shared.RequestResponseMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.MassTransit.RequestResponsePattern.Consumer.Consumers
{
    public class RequestMessageConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            Console.WriteLine($"{context.Message.Text}");
            await context.RespondAsync<ResponseMessage>(new()
            {
                Text = $"{context.Message.MessageNo}. response to request"
            });
        }
    }
}
