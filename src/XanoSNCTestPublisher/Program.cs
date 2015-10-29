using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XanoServiceNotificationCenterTestPublisher
{
    /// <summary>
    /// Resources:
    /// http://www.c-sharpcorner.com/UploadFile/dacca2/http-request-methods-get-post-put-and-delete/
    /// 
    /// </summary>
    class Program
    {
        static void Main()
        {
            SendNotification();
        }

        static async void SendNotification()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:8733/XanoServiceNotificationCenter");
                string json = "{ \"FirmwarePackageVersion\": \"5.0.1.9\", \"FirmwareConfigurationVersion\": \"9.1.1.3.0\" }";
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "notifySubscribers/Roka/FirmwareRelease";
                var response = httpClient.PostAsync(url, stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Error. The status code was " + response.StatusCode);
                }
            }
        }
    }
}
