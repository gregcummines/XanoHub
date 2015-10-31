using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCTestSubscriberLibrary
{
    [DataContract]
    public class ErrorData
    {
        public ErrorData(string reason, string detailedInformation)
        {
            Reason = reason;
            DetailedInformation = detailedInformation;
        }

        [DataMember]
        public string Reason { get; private set; }

        [DataMember]
        public string DetailedInformation { get; private set; }
    }
}
