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
        void CreateNotification(Notification notification);

        [OperationContract]
        List<Notification> GetNotifications();

        [OperationContract]
        void Subscribe(Subscriber subscriber, Notification notification);

        [OperationContract]
        void Unsubscribe(Subscriber subscriber, Notification notification);

        [OperationContract]
        void Notify(Notification notification);
    }
}
