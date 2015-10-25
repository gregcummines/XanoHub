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
        /// <param name="notificationEvent"></param>
        public void CreateNotificationEvent(Publisher publisher, NotificationEvent notificationEvent)
        {
            using (var db = new XanoHubEntities())
            {
                var xPublisher = (from p in db.xPublishers
                                  where p.Name == publisher.Name
                                  select p).SingleOrDefault();
                if (xPublisher == null)
                {
                    db.xPublishers.Add(new xPublisher()
                    {
                        Name = publisher.Name,
                        CreatedDate = DateTime.Now
                    });
                }

                var xNotificationEvent = (from ne in db.xNotificationEvents
                                  where ne.Name == notificationEvent.Name
                                  select ne).SingleOrDefault();
                if (xNotificationEvent == null)
                {
                    db.xNotificationEvents.Add(new xNotificationEvent()
                    {
                        Name = notificationEvent.Name,
                        CreatedDate = DateTime.Now
                    });
                }
                else
                {
                    throw new Exception("Notification: " + notificationEvent.Name + " already exists.");
                }
            }
        }

        /// <summary>
        /// Gets a list of all notification types from the database
        /// </summary>
        /// <returns></returns>
        public List<NotificationEvent> GetNotificationEvents()
        {
            using (var db = new XanoHubEntities())
            {
                return (from ne in db.xNotificationEvents
                          select new NotificationEvent()
                          {
                              Name = ne.Name 
                          }).ToList();
            }
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
        public List<NotificationEvent> GetNotificationsForSubscriber(Subscriber subscriber)
        {
            return null;
        }

        /// <summary>
        /// Gets a list of subscribers for active notifications
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public List<Subscriber> GetSubscribersForNotification(NotificationEvent notification)
        {
            return null;
        }

        /// <summary>
        /// Stores a service subscription to a notification in the database
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="notification"></param>
        public void Subscribe(Subscriber subscriber, NotificationEvent notification)
        {

        }

        /// <summary>
        /// Removes a service subscription to a notification in the database
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="notification"></param>
        public void Unsubscribe(Subscriber subscriber, NotificationEvent notification)
        {

        }

        /// <summary>
        /// Tracks an outgoing hub notification attempt to subscribers in the database
        /// </summary>
        /// <param name="notification"></param>
        public void BeginNotify(NotificationEvent notification)
        {

        }

        /// <summary>
        /// Tracks the successful send of a hub notification to the subscriber in the database
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="subscriber"></param>
        public void EndNotify(NotificationEvent notification, Subscriber subscriber)
        {

        }
    }
}
