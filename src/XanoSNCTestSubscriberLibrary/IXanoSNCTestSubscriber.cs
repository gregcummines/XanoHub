using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace XanoSNCTestSubscriberLibrary
{
    [ServiceContract]
    public interface IXanoSNCTestSubscriber
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "notify/{notificationEvent}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Notify(string notificationEvent, Stream json);
    }
}
