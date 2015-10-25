using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace XanoHubLibrary
{
    /// <summary>
    /// Starkey WCF service to handle notifications and subscriptions between Xano subsystems
    /// Example: 
    /// 1. Roka wants to send a FirmwareRelease notification to subsystems, so Roka creates the 
    ///    FirmwareRelease notification type.
    /// 2. PSM gets notifications and finds that there is a FirmwareRelease notification type.
    ///    It is not necessary, but PSM could check the notification and find that Roka owns it. 
    /// 3. PSM subscribes to the FirmwareRelease notification and gives the hub a Url to call. 
    /// 4. Roka handles a FirmwareRelease and notifies all subscribers (PSM is subscribed)
    /// 5. PSM wants to temporarily not listen to the FirmwareRelease notification, so PSM
    ///    unsubscribes from that notification. 
    /// </summary>
    public class XanoHubService : IXanoHubService
    {
        // Greg 10/25/2015: I wanted to dependency inject the repository interface into the 
        // WCF service constructor using an IOC framework, such as Unity, ServiceMap, 
        // Castlerock, or Ninject. But I can't figure out how to do the registration before this 
        // object is instantiated. So I built a singleton for the repository until this can be 
        // figured out. The downside is that I cannot unit test this component without using 
        // the repository singleton!!! 
        //private IXanoHubRepository hubRepository;

        /// <summary>
        /// Used to create a notification. Any service can create a notification and notify
        /// any subscribers of changes. 
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="notificationEvent"></param>
        public void CreateNotificationEvent(Publisher publisher, NotificationEvent notificationEvent)
        {
            XanoHubRepository.Instance.CreateNotificationEvent(publisher, notificationEvent);
        }

        /// <summary>
        /// Gets the list of notifications supported by the XanoHub
        /// </summary>
        /// <returns></returns>
        public List<NotificationEvent> GetNotificationEvents()
        {
            return XanoHubRepository.Instance.GetNotificationEvents();  
        }

        /// <summary>
        /// Method used to allow a service to send a notification on the notification
        /// </summary>
        /// <param name="notification"></param>
        public void Notify(NotificationEvent notificationEvent)
        {
            // Let the repository know that we are starting a notify, so that a Notification record
            // can be created to track everything
            XanoHubRepository.Instance.BeginNotifyAll(notificationEvent);

            // Find all the subscribers that want to know about this notification
            var subscribers = XanoHubRepository.Instance.GetSubscribersForNotification(notificationEvent);
            foreach(var subscriber in subscribers)
            {
                // todo: call subscriber.NotificationUrl
                XanoHubRepository.Instance.EndNotifySubscriber(notificationEvent, subscriber /*, todo: exceptionDetails */);
            }
        }

        public void Subscribe(Subscriber subscriber, NotificationEvent notification)
        {
            XanoHubRepository.Instance.Subscribe(subscriber, notification);
        }

        public void Unsubscribe(Subscriber subscriber, NotificationEvent notification)
        {
            XanoHubRepository.Instance.Unsubscribe(subscriber, notification);
        }
    }
}
