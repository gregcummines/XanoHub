using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    /// <summary>
    /// Interface used to manage the Xano SNC persistent data
    /// </summary>
    internal class XanoSNCRepository
    {
        private static XanoSNCRepository instance = new XanoSNCRepository();
        private XanoSNCRepository() { }

        public static XanoSNCRepository Instance { get { return instance; } }

        /// <summary>
        /// Stores a new notification type in the database
        /// </summary>
        /// <param name="notificationEvent"></param>
        public void CreateNotificationEvent(Publisher publisher, NotificationEvent notificationEvent)
        {
            using (var db = new XanoSNCEntities())
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
            using (var db = new XanoSNCEntities())
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
            using (var db = new XanoSNCEntities())
            {
                return (from s in db.xSubscribers
                        select new Subscriber()
                        {
                            Name = s.Name
                        }).ToList();
            }
        }

        /// <summary>
        /// Gets an active list of notifications for the subscriber
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public List<NotificationEvent> GetNotificationsForSubscriber(Subscriber subscriber)
        {
            using (var db = new XanoSNCEntities())
            {
                return (from sb in db.xSubscriptions
                        join ne in db.xNotificationEvents on sb.NotificationEventId equals ne.Id
                        join sc in db.xSubscribers on sb.SubscriberId equals sc.Id
                        where sc.Name == subscriber.Name
                        select new NotificationEvent()
                        {
                            Name = ne.Name
                        }).ToList();
            }
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
            using (var db = new XanoSNCEntities())
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
        /// Tracks an outgoing SNC notification attempt to subscribers in the database
        /// </summary>
        /// <param name="notificationEvent"></param>
        public int CreateNotification(Publisher publisher, NotificationEvent notificationEvent)
        {
            // Insert a notification record
            using (var db = new XanoSNCEntities())
            {
                // Lookup the notification event by name 
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

                db.xNotifications.Add(notification);

                db.SaveChanges();
            }

            return 0;
        }

        /// <summary>
        /// Called by the WCF service for each subscriber that we attempted to called
        /// </summary>
        /// <param name="notificationId">Id of the notification record</param>
        /// <param name="subscriber">Subscriber we attempted to contact</param>
        /// <param name="errorMessage">If an error occurred, this will be a non-null string with the message</param>
        public void CreateSubscriptionNotification(int notificationId, Subscriber subscriber, string errorMessage)
        {
            // Find the subscription id by subscriber name and notification event name
            // We need it to create a list of records for each subscriber that we attemped to contact
            using (var db = new XanoSNCEntities())
            {
                var subscriptionDB = (from sb in db.xSubscriptions
                                      join sc in db.xSubscribers on sb.SubscriberId equals sc.Id
                                      select sb).SingleOrDefault();

                if (subscriptionDB == null)
                    throw new Exception("Subscription does not exist!");

                var subscriptionNotification = new xSubscriptionNotification()
                {
                    CreatedDate = DateTime.Now,
                    NotificationError = errorMessage,
                    SubscriptionId = subscriptionDB.Id,
                };

                db.xSubscriptionNotifications.Add(subscriptionNotification);
                db.SaveChanges();
            }
        }
    }
}
