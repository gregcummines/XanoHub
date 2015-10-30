using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    [DataContract]
    public class CreateNotificationEventRequest
    {
        [DataMember]
        public string Publisher { get; set; }
        [DataMember]
        public string NotificationEvent { get; set; }
        //[DataMember]
        //public Stream JsonSchema { get; set; }
    }
}
