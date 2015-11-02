using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace XanoSNCTestSubscriberLibrary
{
    public class XanoSNCTestSubscriber : IXanoSNCTestSubscriber
    {
        public void Notify(string notificationEvent, Stream json)
        {
            if (notificationEvent == null)
                ThrowWebFault("notificationEvent is null", HttpStatusCode.BadRequest);
            if (json == null)
                ThrowWebFault("jsonSchema is null", HttpStatusCode.BadRequest);

            string jsonString = string.Empty;
            try
            {
                using (var reader = new StreamReader(json, Encoding.UTF8, false, 100, true))
                {
                    jsonString = reader.ReadToEnd();
                }

                // todo: Process the notification
            }
            catch (Exception e)
            {
                ThrowWebFault(e.Message, HttpStatusCode.InternalServerError);
            }
        }

        private void ThrowWebFault(string message, HttpStatusCode statusCode)
        {
            var errorData = new ErrorData("Exception", message);
            throw new WebFaultException<ErrorData>(errorData, statusCode);
        }

    }
}
