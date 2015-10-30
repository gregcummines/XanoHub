using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
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
        /// <param name="request"></param>
        public void CreateNotificationEvent(CreateNotificationEventRequest request)
        {
            //string jsonSchema = string.Empty;
            //using (var reader = new StreamReader(request.JsonSchema))
            //{
            //    jsonSchema = reader.ReadToEnd();
            //}
            try
            {
                XanoSNCRepository.Instance.CreateNotificationEvent(request.Publisher, request.NotificationEvent, string.Empty);
            }
            catch(Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }
        }

        /// <summary>
        /// Gets the list of notifications supported by the XanoSNC
        /// </summary>
        /// <returns></returns>
        public List<string> GetNotificationEvents()
        {
            try
            {
                return XanoSNCRepository.Instance.GetNotificationEvents();
            }
            catch(Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }
            return null;
        }
        public string Subscribe(string subscriber, string notification, string notifyUrl)
        {
            try
            {
                return XanoSNCRepository.Instance.Subscribe(subscriber, notification, notifyUrl);
            }
            catch (Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }
            return null;
        }

        public void Unsubscribe(string subscriber, string notificationEvent, string token)
        {
            try
            {
                XanoSNCRepository.Instance.Unsubscribe(subscriber, notificationEvent, token);
            }
            catch (Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }
        }

        /// <summary>
        /// Method used to allow a service to send a notification on the notification
        /// </summary>
        /// <param name="publisher">publisher that published the notification event</param>
        /// <param name="notificationEvent">notification event being published</param>
        /// <param name="json">json object with more information about the notification event</param>
        public void NotifySubscribers(string publisher, string notificationEvent, string json)
        {
            if (publisher == null)
                throw new ArgumentException("publisher must not be null");
            if (notificationEvent == null)
                throw new ArgumentException("notificationEvent must not be null");
            if (json == null)
                throw new ArgumentException("json object must not be null");

            // Let the repository know that we are starting a notify, so that a Notification record
            // can be created to track everything
            int notificationId = 0;
            try
            {
                notificationId = XanoSNCRepository.Instance.CreateNotification(publisher, notificationEvent);
            }
            catch (Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }

            // Find all the subscribers that want to know about this notification
            List<string> subscribers = null;
            try
            {
                subscribers = XanoSNCRepository.Instance.GetSubscribersForNotification(notificationEvent);
            }
            catch (Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }

            foreach (var subscriber in subscribers)
            {
                var errorMessage = string.Empty;    

                try
                {
                    // Try to notify each subscriber
                    NotifySubscriber(publisher, notificationEvent, subscriber, json);
                }
                catch(Exception e)
                {
                    // catch the exception. We are going to create a subscription notification record below
                    // and we need to log the fact that we ran into problems. 
                    errorMessage = e.Message;
                }

                try
                {
                    XanoSNCRepository.Instance.CreateSubscriptionNotification(notificationId, subscriber, errorMessage);
                }
                catch (Exception e)
                {
                    ThrowWebFaultOnRepositoryException(e);
                }
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

        private void ThrowWebFaultOnRepositoryException(Exception e)
        {
            var errorData = new ErrorData("Repository error", e.Message);
            throw new WebFaultException<ErrorData>(errorData, System.Net.HttpStatusCode.InternalServerError);
        }

        public string TestGetMe(string test)
        {
            return "GET Result is " + test;
        }

        public string TestPostMe(string test)
        {
            return "POST Result is " + test;
        }
    }
}
