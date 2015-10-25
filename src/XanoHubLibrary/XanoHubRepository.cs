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
                var publisherId = 0;
                var xPublisher = (from p in db.xPublishers
                                  where p.Name == publisher.Name
                                  select p).SingleOrDefault();
                if (xPublisher == null)
                {
                    var newPublisher = new xPublisher()
                    {
                        Name = publisher.Name,
                        CreatedDate = DateTime.Now
                    };
                    db.xPublishers.Add(newPublisher);
                    publisherId = newPublisher.Id;
                }
                else
                {
                    publisherId = xPublisher.Id;
                }

                var xNotificationEvent = (from ne in db.xNotificationEvents
                                  where ne.Name == notificationEvent.Name
                                  select ne).SingleOrDefault();
                if (xNotificationEvent == null)
                {
                    db.xNotificationEvents.Add(new xNotificationEvent()
                    {
                        Name = notificationEvent.Name,
                        PublisherId = publisherId,
                        CreatedDate = DateTime.Now
                    });
                }
                else
                {
                    throw new Exception("Notification: " + notificationEvent.Name + " already exists.");
                }

                db.SaveChanges();
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
        public void Subscribe(Subscriber subscriber, NotificationEvent notificationEvent)
        {
            using (var db = new XanoHubEntities())
            {
                var xNotificationEvent = (from ne in db.xNotificationEvents
                                          where ne.Name == notificationEvent.Name
                                          select ne).SingleOrDefault();
                if (xNotificationEvent == null)
                {
                    throw new Exception("Notification event with name: " + notificationEvent.Name + " has not been published.");
                }

                var subscriberId = 0;
                var xSubscriber = (from s in db.xSubscribers
                                  where s.Name == subscriber.Name
                                  select s).SingleOrDefault();
                if (xSubscriber == null)
                {
                    var newSubscriber = new xSubscriber()
                    {
                        Name = subscriber.Name,
                        CreatedDate = DateTime.Now
                    };
                    db.xSubscribers.Add(newSubscriber);
                    subscriberId = newSubscriber.Id;
                }
                else
                {
                    subscriberId = xSubscriber.Id;
                }



                var xSubscriptionDB = (from sc in db.xSubscriptions
                                     join sb in db.xSubscribers on sc.SubscriberId equals sb.Id
                                     join ne in db.xNotificationEvents on sc.NotificationEventId equals ne.Id
                                     where ne.Name == notificationEvent.Name && sb.Name == subscriber.Name 
                                     select new
                                     {
                                         SubscriberName = sb.Name, 
                                         NotificationEvent = ne.Name,
                                         NotificationEventId = ne.Id,
                                         SubscriberId = sb.Id
                                     }).SingleOrDefault();
                if (xNotificationEvent == null)
                {
                    db.xSubscriptions.Add(new xSubscription()
                    {
                        NotificationEventId = xSubscriptionDB.NotificationEventId,
                        SubscriberId = xSubscriptionDB.SubscriberId,
                        CreatedDate = DateTime.Now
                    });
                }
                else
                {
                    throw new Exception("Subscription: " + notificationEvent.Name + " already exists.");
                }

                db.SaveChanges();
            }

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
        /// Called by the WCF service when an attempt is made to notify everyone of a notification event
        /// Tracks an outgoing hub notification attempt to subscribers in the database
        /// </summary>
        /// <param name="notificationEvent"></param>
        public void BeginNotifyAll(Publisher publisher, NotificationEvent notificationEvent)
        {
            // todo: find the notification event Id by name, and if it doesn't
            // exist, throw an exception
            using (var db = new XanoHubEntities())
            {
                var notificationEventDB = (from ne in db.xNotificationEvents
                                           where ne.Name == notificationEvent.Name
                                           select ne).SingleOrDefault();
                if (notificationEventDB == null)
                    throw new Exception("Notification by name: " + notificationEvent.Name + " does not exist");

                var notification = new xNotification()
                {
                    NotificationEventId = notificationEventDB.Id,
                    
                    CreatedDate = DateTime.Now
                };
            }
        }

        /// <summary>
        /// Called by the WCF service for each subscriber that we attempted to called
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="notificationEvent"></param>
        /// <param name="subscriber"></param>
        public void EndNotifySubscriber(Publisher publisher, NotificationEvent notificationEvent, Subscriber subscriber)
        {

        }
    }
}
