using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesWebSender
{
    public sealed class MessagesWebSenderOptions
    {
        public string Domain { get; set; }

        public int Port { get; set; }

        public string Guid { get; set; }
    }
}
