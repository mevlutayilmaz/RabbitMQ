using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.MassTransit.Shared.RequestResponseMessage
{
    public class RequestMessage
    {
        public int MessageNo { get; set; }
        public string Text { get; set; }
    }
}
