using System;

namespace ServerlessLoop.Domain
{
    public class ServerlessMessage
    {
        public Guid MessageId { get; set; }

        public int ParentTotal { get; set; }

        public int Amount { get; set; }
    }
}