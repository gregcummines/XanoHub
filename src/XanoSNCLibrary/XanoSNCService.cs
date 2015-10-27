using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace XanoSNCLibrary
{
    /// <summary>
    /// Starkey WCF service to handle notifications and subscriptions between Xano subsystems
    /// Example: 
    /// 1. Roka wants to send a FirmwareRelease notification to subsystems, so Roka creates the 
    ///    FirmwareRelease notification type.
    /// 2. PSM gets notifications and finds that there is a FirmwareRelease notification type.
    ///    It is not necessary, but PSM could check the notification and find that Roka owns it. 
    /// 3. PSM subscribes to the FirmwareRelease notification and gives the SNC a Url to call. 
    /// 4. Roka handles a FirmwareRelease and notifies all subscribers (PSM is subscribed)
    /// 5. PSM wants to temporarily not listen to the FirmwareRelease notification, so PSM
    ///    unsubscribes from that notification. 
    /// </summary>
    public class XanoSNCService : IXanoSNCService
    {
        // Greg 10/25/2015: I wanted to dependency inject the repository interface into the 
        // WCF service constructor using an IOC framework, such as Unity, ServiceMap, 
        // Castlerock, or Ninject. But I can't figure out how to do the registration before this 
        // object is instantiated. So I built a singleton for the repository until this can be 
        // figured out. The downside is that I cannot unit test this component without using 
        // the repository singleton!!! 
        //private IXanoSNCRepository SNCRepository;

        /// <summary>
        /// Used to create a notification. Any service can create a notification and notify
        /// any subscribers of changes. 
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="notificationEvent"></param>
        public void CreateNotificationEvent(string publisher, string notificationEvent, string jsonSchema)
        {
            XanoSNCRepository.Instance.CreateNotificationEvent(publisher, notificationEvent, jsonSchema);
        }

        /// <summary>
        /// Gets the list of notifications supported by the XanoSNC
        /// </summary>
        /// <returns></returns>
        public List<string> GetNotificationEvents()
        {
            return XanoSNCRepository.Instance.GetNotificationEvents();  
        }
        public void Subscribe(string subscriber, string notification, string notifyUrl)
        {
            XanoSNCRepository.Instance.Subscribe(subscriber, notification, notifyUrl);
        }

        public void Unsubscribe(string subscriber, string notification)
        {
            XanoSNCRepository.Instance.Unsubscribe(subscriber, notification);
        }

        /// <summary>
        /// Method used to allow a service to send a notification on the notification
        /// </summary>
        /// <param name="publisher">publisher that published the notification event</param>
        /// <param name="notificationEvent">notification event being published</param>
        /// <param name="json">json object with more information about the notification event</param>
        public void NotifySubscribers(string publisher, string notificationEvent, string json)
        {
            // Let the repository know that we are starting a notify, so that a Notification record
            // can be created to track everything
            var notificationId = XanoSNCRepository.Instance.CreateNotification(publisher, notificationEvent);

            // Find all the subscribers that want to know about this notification
            var subscribers = XanoSNCRepository.Instance.GetSubscribersForNotification(notificationEvent);
            foreach(var subscriber in subscribers)
            {
                var errorMessage = string.Empty;    // todo: fill in the string

                try
                {
                    NotifySubscriber(publisher, notificationEvent, subscriber, json);
                }
                catch(Exception e)
                {
                    errorMessage = e.Message;
                }

                XanoSNCRepository.Instance.CreateSubscriptionNotification(notificationId, subscriber, errorMessage);
            }
        }

        async private void NotifySubscriber(string publisher, string notificationEvent, string subscriber, string json)
        {
            var notifyUrl = XanoSNCRepository.Instance.GetUrlFromSubscription(notificationEvent, subscriber);

            using (var httpClient = new HttpClient())
            {
                //string json = "{ 'FirmwarePackageVersion': '5.0.1.9', 'FirmwareConfigurationVersion': '9.1.1.3.0' }";
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(notifyUrl, null);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Status code is " + response.StatusCode);
                }
            }
        }
    }
}
