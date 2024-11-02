using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFConnectionMessaging.Models
{
    internal class Packet
    {
        public byte[] Data { get; set; }

        public string EndpointIP { get; set; } = String.Empty;

        public int EndpointPort { get; set; }
    }
}
