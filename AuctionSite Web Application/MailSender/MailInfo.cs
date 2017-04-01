using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender
{
    public class MailInfo
    {
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
