using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XanoSNCTestSubscriberWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if !DEBUG
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new XanoSNCTestSubscriberWindowsService()
            };
            ServiceBase.Run(ServicesToRun);
#else
            // Make a subscription to a notification event
            using (var httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                var url = "http://localhost:8733/XanoServiceNotificationCenter/getNotificationEvents";
                var httpResponseMessage = httpClient.GetAsync(url);
                var result = httpResponseMessage.Result;
                var jsonResult = result.Content.ReadAsStringAsync().Result;
                dynamic jsonResponse = JsonConvert.DeserializeObject(jsonResult);

                // UriTemplate: subscribe/{subscriber}/{notificationEvent}
                url = "http://localhost:8733/XanoServiceNotificationCenter/subscribe/testSubscriber/FirmwareRelease";

                string json = "http://localhost:8733/XanoTestSubscriber/notify/FirmwareRelease";
                var stringContent = new StringContent(json, Encoding.UTF8);
                httpResponseMessage = httpClient.PostAsync(url, stringContent);
                result = httpResponseMessage.Result;
                jsonResult = result.Content.ReadAsStringAsync().Result;
                // The result is a token that will be need to be used if we want to unsubscribe. 
                // This will prevent others from unsubscribing to notification events that we subscribed to.
                jsonResponse = JsonConvert.DeserializeObject(jsonResult);
            }

            var service = new XanoSNCTestSubscriberWindowsService();
            service.DebugThisService();
#endif
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
