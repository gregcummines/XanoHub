using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XanoHubLibrary
{
    [DataContract]
    public class Subscriber
    {
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string NotificationUrl { get; set; }
    }
}
