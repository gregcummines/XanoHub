using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace XanoSNCLibrary
{
    [ServiceContract]
    public interface IXanoSNCService
    {
        /// <summary>
        /// Allows a publisher to create a notification event type that subscribers can subscribe to
        /// </summary>
        /// <param name="publisher">Publisher creating the notification event</param>
        /// <param name="notificationEvent">Notification event to be created</param>
        /// <param name="jsonSchema">Schema of json object that will later be sent via NotifySubscribers</param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "createNotificationEvent",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void CreateNotificationEvent(string publisher, string notificationEvent, string jsonSchema);

        /// <summary>
        /// Returns a list of persisted notification events
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet]
        List<string> GetNotificationEvents();

        /// <summary>
        /// Allows a subscriber to subscribe to a notification event
        /// </summary>
        /// <param name="subscriber">Subscriber requesting to subscribe</param>
        /// <param name="notificationEvent">Notification event requesting to be subscribed to</param>
        /// <param name="notifyUrl">
        /// A RESTful API Url to be called when a notification is sent by a publisher
        /// This Url will have POST called on it, since a notification is being posted.
        /// Since REST uses resources, an example of a Url is: http://domain.com/service1/notification/firmwarerelease
        /// With this Url the firmware release json object would be put in the body of the POST
        /// Here is an example of how to design a good REST API, but there are many more: http://mark-kirby.co.uk/2013/creating-a-true-rest-api/
        /// </param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "subscribe",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Subscribe(string subscriber, string notificationEvent, string notifyUrl);

        /// <summary>
        /// Allows a subscriber to unsubscribe from a notification event. 
        /// </summary>
        /// <param name="subscriber">Subscriber wanting to unsubscribe</param>
        /// <param name="notificationEvent">Notification event to unsubscribe from</param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "unsubscribe",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Unsubscribe(string subscriber, string notificationEvent);

        /// <summary>
        /// Allows a publisher to notify subscribers of a notification event
        /// </summary>
        /// <param name="publisher">The publisher that is notifying subscribers</param>
        /// <param name="notificationEvent">The notification event being published</param>
        /// <param name="json">An object representing a resource that will be POSTED to the Url in the subscription</param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "notifySubscribers/{publisher}/{notificationEvent}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void NotifySubscribers(string publisher, string notificationEvent, string json);
    }
}
