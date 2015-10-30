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

        /// <summary>
        /// Allows a publisher to create a notification event type that subscribers can subscribe to
        /// </summary>
        /// <param name="request"></param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "createNotificationEvent",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void CreateNotificationEvent(CreateNotificationEventRequest request);

        /// <summary>
        /// Returns a list of persisted notification events
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
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
            UriTemplate = "subscribe/{subscriber}/{notificationEvent}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Subscribe(string subscriber, string notificationEvent, string notifyUrl);

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
