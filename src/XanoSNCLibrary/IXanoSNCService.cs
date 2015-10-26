using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace XanoSNCLibrary
{
    [ServiceContract]
    public interface IXanoSNCService
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
