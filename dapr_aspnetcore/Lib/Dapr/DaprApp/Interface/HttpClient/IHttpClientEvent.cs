using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DaprApp.Interface.HttpClient
{
    public interface IHttpClientEvent
    {
        /// <summary> 
        /// 發起Http method為Get的HttpClient請求。
        /// </summary>
        /// <remarks>
        /// 透過DaprClient使用HttpClient進行服務之間呼叫。
        /// </remarks>
        /// <param name="appId">欲呼叫請求Dapr App Id</param>
        /// <param name="requestUrl">呼叫API服務Path</param>
        /// <returns>回傳<see cref="HttpResponseMessage"/>物件</returns>
        Task<HttpResponseMessage> HttpRequestByGet(string appId, string requestUrl);

        /// <summary> 
        /// 發起Http method為Post的HttpClient請求。
        /// </summary>
        /// <remarks>
        /// 透過DaprClient使用HttpClient進行服務之間呼叫。
        /// </remarks>
        /// <param name="requestData">Http body內Data物件</param>
        /// <param name="appId">欲呼叫請求Dapr App Id</param>
        /// <param name="requestUrl">呼叫API服務Path</param>
        /// <returns>回傳<see cref="HttpResponseMessage"/>物件</returns>
        Task<HttpResponseMessage> HttpResponseByPost<Target>(Target requestData, string appId, string requestUrl);

        /// <summary> 
        /// 發起Http method為Delete的HttpClient請求。
        /// </summary>
        /// <remarks>
        /// 透過DaprClient使用HttpClient進行服務之間呼叫。
        /// </remarks>
        /// <param name="appId">欲呼叫請求Dapr App Id</param>
        /// <param name="requestUrl">呼叫API服務Path</param>
        /// <returns>回傳<see cref="HttpResponseMessage"/>物件</returns>
        Task<HttpResponseMessage> HttpResponseByDelete(string appId, string requestUrl);

        /// <summary> 
        /// 發起Http method為Put的HttpClient請求。
        /// </summary>
        /// <remarks>
        /// 透過DaprClient使用HttpClient進行服務之間呼叫。
        /// </remarks>
        /// <param name="requestData">Http body內Data物件</param>
        /// <param name="appId">欲呼叫請求Dapr App Id</param>
        /// <param name="requestUrl">呼叫API服務Path</param>
        /// <returns>回傳<see cref="HttpResponseMessage"/>物件</returns>
        Task<HttpResponseMessage> HttpResponseByPut<Target>(Target requestData, string appId, string requestUrl);
    }
}
