using Dapr.Client;
using DaprApp.Interface.HttpClient;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DaprApp.Services
{
    public class DaprHttpClientServices : IHttpClientEvent
    {
        public DaprHttpClientServices() { }

        public async Task<HttpResponseMessage> HttpRequestByGet(
            string appId,
            string requestUrl
        )
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (HttpClient client = DaprClient.CreateInvokeHttpClient(appId: appId))
            {
                string queryURL = requestUrl;

                response = await client.GetAsync(queryURL);
            }

            return response;
        }

        public async Task<HttpResponseMessage> HttpResponseByDelete(
            string appId,
            string requestUrl
        )
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (HttpClient client = DaprClient.CreateInvokeHttpClient(appId: appId))
            {
                string queryURL = requestUrl;

                response = await client.DeleteAsync(queryURL);
            }

            return response;
        }

        public async Task<HttpResponseMessage> HttpResponseByPost<Target>(
            Target requestData,
            string appId,
            string requestUrl
        )
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (HttpClient client = DaprClient.CreateInvokeHttpClient(appId: appId))
            {
                string queryURL = requestUrl;

                response = await client.PostAsJsonAsync(queryURL, requestData);
            }

            return response;
        }

        public async Task<HttpResponseMessage> HttpResponseByPut<Target>(
            Target requestData,
            string appId,
            string requestUrl
        )
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (HttpClient client = DaprClient.CreateInvokeHttpClient(appId: appId))
            {
                string queryURL = requestUrl;

                response = await client.PostAsJsonAsync(queryURL, requestData);
            }

            return response;
        }
    }
}
