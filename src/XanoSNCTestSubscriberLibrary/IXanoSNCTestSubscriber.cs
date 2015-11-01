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
    /// <summary>
    /// Web API interface to get notifications from the XanoServiceNotificationCenter
    /// </summary>
    [ServiceContract]
    public interface IXanoSNCTestSubscriber
    {
        /// <summary>
        /// Method that XanoServiceNotificationCenter will call when a publisher sends out a notification 
        /// </summary>
        /// <param name="notificationEvent">The notification event being published</param>
        /// <param name="json">A json object containing specifics about the notification</param>
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
