using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Int.Domain.Entities
{
    public class Message
    {
        public string From { get; set; }
        public string To { get; set; }
        public List<string> Bcs { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
