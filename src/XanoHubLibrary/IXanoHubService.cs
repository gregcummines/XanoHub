using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace XanoHubLibrary
{
    [ServiceContract]
    public interface IXanoHubService
    {
        [OperationContract]
        void CreateNotificationEvent(Publisher publisher, NotificationEvent notificationEvent);

        [OperationContract]
        List<NotificationEvent> GetNotificationEvents();

        [OperationContract]
        void Subscribe(Subscriber subscriber, NotificationEvent notificationEvent);

        [OperationContract]
        void Unsubscribe(Subscriber subscriber, NotificationEvent notificationEvent);

        [OperationContract]
        void NotifySubscribers(Publisher publisher, NotificationEvent notificationEvent);
    }
}
