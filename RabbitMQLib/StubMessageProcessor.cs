using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQLib
{
    public class StubMessageProcessor : IMessageProcessor
    {
        public void Process(IMessage message)
        {
            Console.WriteLine("Processor message: " + message.Body.ToString());
        }
    }
}