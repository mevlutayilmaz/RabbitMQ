using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.MassTransit.Shared.Messages
{
    public class Message : IMessage
    {
        public string Text { get; set; }
    }
}
