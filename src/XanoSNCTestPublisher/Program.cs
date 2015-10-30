using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers; // For AuthenticationHeaderValue

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
            TestGetMe();
            TestPostMe();
            Publisher_CreateNotificationEvent();
            GetNotificationEvents();
            //Publisher_SendNotification();
        }

        private static void TestGetMe()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                var url = "http://localhost:8733/XanoServiceNotificationCenter/testGetMe/myString";
                var httpResponseMessage = httpClient.GetAsync(url);
                var result = httpResponseMessage.Result;
                var jsonResult = result.Content.ReadAsStringAsync().Result;
                dynamic jsonResponse = JsonConvert.DeserializeObject(jsonResult);


                //if (response.IsSuccessStatusCode)
                //{
                //    Log("Success");
                //}
                //else
                //{
                //    Log("Error. The status code was " + response.StatusCode);
                //}
            }
        }

        private static void TestPostMe()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                var url = "http://localhost:8733/XanoServiceNotificationCenter/testPostMe";
                string json = "\"This is a test\"";
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponseMessage = httpClient.PostAsync(url, stringContent);
                var result = httpResponseMessage.Result;
                var jsonResult = result.Content.ReadAsStringAsync().Result;
                dynamic jsonResponse = JsonConvert.DeserializeObject(jsonResult);

                //if (response.IsSuccessStatusCode)
                //{
                //    Log("Success");
                //}
                //else
                //{
                //    Log("Error. The status code was " + response.StatusCode);
                //}
            }
        }

        static void Publisher_CreateNotificationEvent()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                //var jsonSchema = "{\"$schema\":\"http://json-schema.org/draft-04/schema#\",\"id\":\"http://jsonschema.net\",\"type\":\"object\",\"properties\":{\"FirmwarePackageVersion\":{\"id\":\"http://jsonschema.net/FirmwarePackageVersion\",\"type\":\"string\"},\"FirmwareConfigurationVersion\":{\"id\":\"http://jsonschema.net/FirmwareConfigurationVersion\",\"type\":\"string\"}},\"required\":[\"FirmwarePackageVersion\",\"FirmwareConfigurationVersion\"]}";
                //var json = "{ \"Publisher\": \"Roka\", \"NotificationEvent\": \"FirmwareRelease\", \"JsonSchema\": " + jsonSchema + "}"; 

                var json = "{ \"Publisher\": \"Roka\", \"NotificationEvent\": \"FirmwareRelease\" }";
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://localhost:8733/XanoServiceNotificationCenter/createNotificationEvent";
                var response = httpClient.PostAsync(url, stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    Log("Success");
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    Log(errorMessage);
                }
            }
        }

        static void GetNotificationEvents()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                var url = "http://localhost:8733/XanoServiceNotificationCenter/getNotificationEvents";
                var httpResponseMessage = httpClient.GetAsync(url);
                var result = httpResponseMessage.Result;
                var jsonResult = result.Content.ReadAsStringAsync().Result;
                dynamic jsonResponse = JsonConvert.DeserializeObject(jsonResult);
            }
        }

        static async void Publisher_SendNotification()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                string json = "{ \"FirmwarePackageVersion\": \"5.0.1.9\", \"FirmwareConfigurationVersion\": \"9.1.1.3.0\" }";
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://localhost:8733/XanoServiceNotificationCenter/notifySubscribers/Roka/FirmwareRelease";
                var response = httpClient.PostAsync(url, stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    Log("Success");
                }
                else
                {
                    Log("Error. The status code was " + response.StatusCode);
                }
            }
        }

        static void Log(string message)
        {
            Debug.WriteLine(message);
            Console.WriteLine(message);
        }
    }
}
