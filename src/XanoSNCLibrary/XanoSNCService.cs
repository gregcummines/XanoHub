﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

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
        private IXanoSNCRepository _xanoSNCRepository;
        private IMailService _mailService;

        public XanoSNCService(IXanoSNCRepository xanoSNCRepository, IMailService mailService)
        {
            _xanoSNCRepository = xanoSNCRepository;
            _mailService = mailService;
        }
        
        /// <summary>
        /// Used to create a notification. Any service can create a notification and notify
        /// any subscribers of changes. 
        /// </summary>
        /// <param name="request"></param>
        public string CreateNotificationEvent(string publisher, string notificationEvent, Stream jsonSchema)
        {
            if (publisher == null)
                ThrowWebFault("publisher is null", HttpStatusCode.BadRequest);
            if (notificationEvent == null)
                ThrowWebFault("notificationEvent is null", HttpStatusCode.BadRequest);
            if (jsonSchema == null)
                ThrowWebFault("jsonSchema is null", HttpStatusCode.BadRequest);

            string jsonSchemaString = string.Empty;
            string token = string.Empty;
            try
            {
                using (var reader = new StreamReader(jsonSchema, Encoding.UTF8, false, 100, true))
                {
                    jsonSchemaString = reader.ReadToEnd();
                }
            }
            catch(Exception e)
            {
                ThrowWebFault(e.Message, HttpStatusCode.InternalServerError);
            }

            try
            {
                token = _xanoSNCRepository.CreateNotificationEvent(publisher, notificationEvent, jsonSchemaString);
            }
            catch(Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }
            return token;
        }

        public void UpdateNotificationEventJsonSchema(string token, Stream jsonSchema)
        {
            if (jsonSchema == null)
                ThrowWebFault("jsonSchema is null", HttpStatusCode.BadRequest);

            string jsonSchemaString = string.Empty;

            try
            {
                using (var reader = new StreamReader(jsonSchema, Encoding.UTF8, false, 100, true))
                {
                    jsonSchemaString = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                ThrowWebFault(e.Message, HttpStatusCode.InternalServerError);
            }

            try
            {
                _xanoSNCRepository.UpdateNotificationEventJsonSchema(token, jsonSchemaString);
            }
            catch (Exception e)
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
                return _xanoSNCRepository.GetNotificationEvents();
            }
            catch(Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }
            return null;
        }

        public Stream GetNotificationEventMessageSchema(string notificationEvent)
        {
            var jsonSchemaString = string.Empty;
            try
            {
                jsonSchemaString = _xanoSNCRepository.GetNotificationEventMessageSchema(notificationEvent);
            }
            catch (Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }

            WebOperationContext.Current.OutgoingResponse.ContentType =
                "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(jsonSchemaString));
        }

        public string Subscribe(string subscriber, string notification, string emailAddress, Stream notifyUrl)
        {
            string notifyUrlString = string.Empty;
            using (var reader = new StreamReader(notifyUrl, Encoding.UTF8, false, 100, true))
            {
                notifyUrlString = reader.ReadToEnd();
            }

            try
            {
                return _xanoSNCRepository.Subscribe(subscriber, notification, emailAddress, notifyUrlString);
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
                _xanoSNCRepository.Unsubscribe(subscriber, notificationEvent, token);
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
        /// <param name="token">Token required to verify that you are the publisher of this notification event</param>
        /// <param name="json">json object with more information about the notification event</param>
        public void NotifySubscribers(string publisher, string notificationEvent, string token, Stream json)
        {
            if (publisher == null)
                ThrowWebFault("publisher is null", HttpStatusCode.BadRequest);
            if (notificationEvent == null)
                ThrowWebFault("notificationEvent is null", HttpStatusCode.BadRequest);
            if (token == null)
                ThrowWebFault("token is null", HttpStatusCode.BadRequest);
            if (json == null)
                ThrowWebFault("json is null", HttpStatusCode.BadRequest);

            string jsonString = string.Empty;
            using (var reader = new StreamReader(json, Encoding.UTF8, false, 100, true))
            {
                jsonString = reader.ReadToEnd();
            }

            int notificationId = 0;
            try
            {
                // Verify that this is the publisher that created the event
                if (_xanoSNCRepository.NotificationEventHasToken(notificationEvent, token))
                {
                    // Let the repository know that we are starting a notify, so that a Notification record
                    // can be created to track everything
                    notificationId = _xanoSNCRepository.CreateNotification(publisher, notificationEvent, jsonString);
                }
            }
            catch (Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }

            if (notificationId == 0)
            {
                ThrowWebFault("Token " + token + " does not exist for the notificationEvent " + notificationEvent + " for publisher " + publisher, HttpStatusCode.BadRequest);
            }

            // Find all the subscribers that want to know about this notification
            List<string> subscribers = null;
            try
            {
                subscribers = _xanoSNCRepository.GetSubscribersForNotification(notificationEvent);
            }
            catch (Exception e)
            {
                ThrowWebFaultOnRepositoryException(e);
            }

            // Attempt to notify each subscriber
            foreach (var subscriber in subscribers)
            {
                NotifySubscriber(notificationId, publisher, notificationEvent, subscriber, jsonString);
            }
        }

        async private void NotifySubscriber(
            int notificationId, 
            string publisher, 
            string notificationEvent, 
            string subscriber, 
            string json)
        {
            try
            {
                var notifyUrl = _xanoSNCRepository.GetUrlFromSubscription(notificationEvent, subscriber);

                // Start a subscription notification record. We'll update it when we after we attempt to 
                // reach the subscriber
                var subscriptionNotificationId = _xanoSNCRepository.CreateSubscriptionNotification(notificationId, subscriber); 

                using (var httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
                {
                    var stringContent = new StringContent(json, Encoding.UTF8);
                    notifyUrl = HttpUtility.UrlPathEncode(notifyUrl);

                    // POST the json object to the Url provided by the subscriber
                    var response = await httpClient.PostAsync(notifyUrl, stringContent);
                    if (!response.IsSuccessStatusCode)
                    {
                        var jsonResult = response.Content.ReadAsStringAsync().Result;
                        NotifySubscriberOfError(subscriber, notificationEvent, publisher, notifyUrl, json, jsonResult);
                        _xanoSNCRepository.UpdateSubscriptionNotificationWithError(subscriptionNotificationId, jsonResult);
                    }
                }
            }
            catch(Exception e)
            {
                ThrowWebFault(e.Message, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Notifies the subscriber via e-mail that the service could not be reached for notification
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="notificationEvent"></param>
        /// <param name="publisher"></param>
        /// <param name="notifyUrl"></param>
        /// <param name="json"></param>
        /// <param name="serverResponse"></param>
        private async void NotifySubscriberOfError(
            string subscriber, 
            string notificationEvent, 
            string publisher, 
            string notifyUrl, 
            string json, 
            string serverResponse)
        {
            string emailAddress = _xanoSNCRepository.GetEMailFromSubscription(notificationEvent, subscriber);
            if (IsValidEmail(emailAddress))
            {
                var errorMessage = "<h2>Could not reach " + subscriber + ".</h2><p> " + publisher + " published a " + notificationEvent + " notification, but the XanoServiceNotificationCenter could not relay that to: " + notifyUrl +
                    ". </p><p>Here is extra information that was sent with the message: " + json + "</p>.<p>Detailed error information: " + serverResponse + "</p>";
                await _mailService.SendEmailAsync(emailAddress, "Error contacting " + subscriber, errorMessage);
            }
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ThrowWebFault(string message, HttpStatusCode statusCode)
        {
            var errorData = new ErrorData("Exception", message);
            throw new WebFaultException<ErrorData>(errorData, statusCode);
        }

        private void ThrowWebFaultOnRepositoryException(Exception e)
        {
            var errorData = new ErrorData("Repository error", e.Message);
            throw new WebFaultException<ErrorData>(errorData, HttpStatusCode.InternalServerError);
        }

        public void TestNotifySubscriber(string subscriber, string notificationEvent, string subscriptionToken, string json)
        {
            // todo: Make sure this is the subscriber by validating the subscription token
            if (_xanoSNCRepository.SubscriptionNotificationHasToken(subscriber, notificationEvent, subscriptionToken))
            {
                NotifySubscriber(/* no notificationId since we are not storing this test */0, 
                    "TestPublisher", "testNotificationEvent", subscriber, json);
            }
            else
            {
                ThrowWebFault("No subscription found with subscription token " + subscriptionToken + ", for subscriber " + subscriber + ", for notificationEvent " + notificationEvent, HttpStatusCode.BadRequest);
            }            
        }
    }
}
