using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace NamecheapDynDnsUpdater
{
    public class IPInfo
    {
        public string AdapterId { get; set; }
        public string IPAddress { get; set; }
        public string AdapterName { get; set; }
        public OperationalStatus Status { get; set; }
    }
}
