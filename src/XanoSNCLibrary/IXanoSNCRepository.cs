using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    public interface IXanoSNCRepository
    {
        string CreateNotificationEvent(string publisher, string notificationEvent, string jsonSchema);

        /// <summary>
        /// Gets a list of all notification types from the database
        /// </summary>
        /// <returns></returns>
        List<string> GetNotificationEvents();

        /// <summary>
        /// Gets a list of all active subscribers
        /// </summary>
        /// <returns></returns>
        List<Subscriber> GetSubscribers();
        
        string GetNotificationEventMessageSchema(string notificationEvent);

        /// <summary>
        /// Gets an active list of notifications for the subscriber
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        List<NotificationEvent> GetNotificationsForSubscriber(Subscriber subscriber);

        string GetUrlFromSubscription(string notificationEvent, string subscriber);

        bool NotificationEventHasToken(string notificationEvent, string token);

        /// <summary>
        /// Gets a list of subscribers for active notifications
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        List<string> GetSubscribersForNotification(string notification);

        bool SubscriptionNotificationHasToken(string subscriber, string notificationEvent, string token);

        /// <summary>
        /// Stores a service subscription to a notification in the database
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="notificationEvent"></param>
        /// <param name="notifyUrl"></param>
        /// <returns>A token that must be used when unsubscribing</returns>
        string Subscribe(string subscriber, string notificationEvent, string emailAddress, string notifyUrl);

        string GetEMailFromSubscription(string notificationEvent, string subscriber);


        void UpdateSubscriptionNotificationWithError(int subscriptionNotificationId, string errorMessage);

        int CreateSubscriptionNotification(int notificationId, string subscriber);

        /// <summary>
        /// Removes a service subscription to a notification in the database
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="notificationEvent"></param>
        /// <param name="token">A token issued by the subscribe method</param>
        void Unsubscribe(string subscriber, string notificationEvent, string token);

        /// <summary>
        /// Called by the WCF service when an attempt is made to notify everyone of a notification event
        /// Tracks an outgoing SNC notification attempt to subscribers in the database
        /// </summary>
        /// <param name="notificationEvent"></param>
        int CreateNotification(string publisher, string notificationEvent, string message);

        /// <summary>
        /// Called by the WCF service for each subscriber that we attempted to called
        /// </summary>
        /// <param name="notificationId">Id of the notification record</param>
        /// <param name="subscriber">Subscriber we attempted to contact</param>
        /// <param name="errorMessage">If an error occurred, this will be a non-null string with the message</param>
        void CreateSubscriptionNotification(int notificationId, string subscriber, string errorMessage);

        /// <summary>
        /// Updates the schema for this notification event 
        /// </summary>
        /// <param name="token">Token of the notification event that was returned in CreateNotificationEvent</param>
        /// <param name="jsonSchemaString">new json schema</param>
        void UpdateNotificationEventJsonSchema(string token, string jsonSchemaString);
    }
}
