using System;
using System.Collections.Generic;
using System.IO;
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
        /// Test GET Method
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "testGetMe/{test}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string TestGetMe(string test);

        /// <summary>
        /// Test POST Method
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "testPostMe",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string TestPostMe(string test);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "testPostStream/{myString}",
        ResponseFormat = WebMessageFormat.Json)]
        void TestPostStream(string myString, Stream stream);

        /// <summary>
        /// Allows a publisher to create a notification event type that subscribers can subscribe to
        /// </summary>
        /// <param name="request"></param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "createNotificationEvent/{publisher}/{notificationEvent}",
            ResponseFormat = WebMessageFormat.Json)]
        string CreateNotificationEvent(string publisher, string notificationEvent, Stream jsonSchema);

        /// <summary>
        /// Returns a list of persisted notification events
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(
            UriTemplate = "getNotificationEvents",
            ResponseFormat = WebMessageFormat.Json)]
        List<string> GetNotificationEvents();

        /// <summary>
        /// Gets the json schema associated with the message sent with a notify is sent out
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet]
        Stream GetNotificationEventMessageSchema(string notificationEvent);

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
            UriTemplate = "subscribe/{subscriber}/{notificationEvent}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Subscribe(string subscriber, string notificationEvent, Stream notifyUrl);

        /// <summary>
        /// Allows a subscriber to unsubscribe from a notification event. 
        /// </summary>
        /// <param name="subscriber">Subscriber wanting to unsubscribe</param>
        /// <param name="notificationEvent">Notification event to unsubscribe from</param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "unsubscribe/{subscriber}/{notificationEvent}/{token}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Unsubscribe(string subscriber, string notificationEvent, string token);

        /// <summary>
        /// Allows a publisher to notify subscribers of a notification event
        /// </summary>
        /// <param name="publisher">The publisher that is notifying subscribers</param>
        /// <param name="notificationEvent">The notification event being published</param>
        /// <param name="token">A token that was given when the notification event was created.
        /// This is required so that only the publisher that created the event can notify others</param>
        /// <param name="json">An object representing a resource that will be POSTED to the Url in the subscription</param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "notifySubscribers/{publisher}/{notificationEvent}/{token}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void NotifySubscribers(string publisher, string notificationEvent, string token, string json);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "testNotifySubscribers/{subscriber}/{notificationEvent}/{subscriptionToken}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void TestNotifySubscriber(string subscriber, string notificationEvent, string subscriptionToken, string json);
    }
}
