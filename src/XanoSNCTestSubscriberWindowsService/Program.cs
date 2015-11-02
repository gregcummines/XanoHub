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

                
            }


            var service = new XanoSNCTestSubscriberWindowsService();
            service.DebugThisService();
#endif
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
