using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    [DataContract]
    public class NotificationEvent
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string JsonSchema { get; set; }
        [DataMember]
        public string Publisher { get; set; }
    }
}
