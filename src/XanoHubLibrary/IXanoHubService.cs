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
        void CreateNotificationEvent(Publisher publisher, NotificationEvent notification);

        [OperationContract]
        List<NotificationEvent> GetNotificationEvents();

        [OperationContract]
        void Subscribe(Subscriber subscriber, NotificationEvent notification);

        [OperationContract]
        void Unsubscribe(Subscriber subscriber, NotificationEvent notification);

        [OperationContract]
        void Notify(NotificationEvent notification);
    }
}
