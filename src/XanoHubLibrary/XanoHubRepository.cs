using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XanoHubLibrary
{
    /// <summary>
    /// Interface used to manage the Xano Hub persistent data
    /// </summary>
    internal class XanoHubRepository
    {
        private static XanoHubRepository instance = new XanoHubRepository();
        private XanoHubRepository() { }

        public static XanoHubRepository Instance { get { return instance; } }

        /// <summary>
        /// Stores a new notification type in the database
        /// </summary>
        /// <param name="notification"></param>
        public void CreateNotification(Notification notification)
        {

        }

        /// <summary>
        /// Gets a list of all notification types from the database
        /// </summary>
        /// <returns></returns>
        public List<Notification> GetNotifications()
        {
            return null;
        }

        /// <summary>
        /// Gets a list of all active subscribers
        /// </summary>
        /// <returns></returns>
        public List<Subscriber> GetSubscribers()
        {
            return null;
        }

        /// <summary>
        /// Gets an active list of notifications for the subscriber
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public List<Notification> GetNotificationsForSubscriber(Subscriber subscriber)
        {
            return null;
        }

        /// <summary>
        /// Gets a list of subscribers for active notifications
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public List<Subscriber> GetSubscribersForNotification(Notification notification)
        {
            return null;
        }

        /// <summary>
        /// Stores a service subscription to a notification in the database
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="notification"></param>
        public void Subscribe(Subscriber subscriber, Notification notification)
        {

        }

        /// <summary>
        /// Removes a service subscription to a notification in the database
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="notification"></param>
        public void Unsubscribe(Subscriber subscriber, Notification notification)
        {

        }

        /// <summary>
        /// Tracks an outgoing hub notification attempt to subscribers in the database
        /// </summary>
        /// <param name="notification"></param>
        public void BeginNotify(Notification notification)
        {

        }

        /// <summary>
        /// Tracks the successful send of a hub notification to the subscriber in the database
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="subscriber"></param>
        public void EndNotify(Notification notification, Subscriber subscriber)
        {

        }
    }
}
