using Dapr.Client;
using DaprApp.Interface.HttpClient;
using DaprApp.Models;
using DataModel.EventBase;
using DataModel.NewsMessage;
using DataModel.Response;
using MainWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MainWebApi.Area.News.Controller.Activity
{
    [Area("News-Activity")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class NewsActivityHttpClientController : ControllerBase
    {
        private readonly ILogger<NewsActivityHttpClientController> log;
        private readonly AppServicesName AppServicesName;
        private readonly IHttpClientEvent _httpClient;

        private const string AreaControllerPath = "/Activity/ActivityNewsEvent";

        public NewsActivityHttpClientController(
            ILogger<NewsActivityHttpClientController> logger,
            IOptionsMonitor<AppServicesName> options,
            IHttpClientEvent httpClientEvent
        )
        {
            log = logger;
            AppServicesName = options.CurrentValue;
            _httpClient = httpClientEvent;
        }

        /// <summary>
        /// News Web API進行兩個API之間溝通，進行公告訊息查詢請求。
        /// </summary>
        /// <remarks>
        /// 透過跟自己對應的dapr sidecar跟News Web API dapr sidecar進行溝通。
        /// </remarks>
        /// <param name="Id">公告編號</param>
        /// <returns></returns>
        [HttpGet("QueryNewsMessage")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> QueryNewsMessageHandler(string Id)
        {
            log.LogInformation($"收到查詢公告內容請求，依據公告編號: {Id}");

            EventDataBase result;
            HttpResponseMessage response = new HttpResponseMessage();

            // [area]/[controller]/[template]
            string queryURL = $"{AreaControllerPath}/Query?Id={Id}";

            response = await _httpClient.HttpRequestByGet(
                appId: AppServicesName.DaprApp_NewsName,
                requestUrl: queryURL
            );

            if (!response.IsSuccessStatusCode)
            {
                log.LogWarning("請求過程發生錯誤，事件請求已終止執行。");
                throw new Exception("The error has occurred.");
            }

            result = await response.Content.ReadFromJsonAsync<EventDataBase>();

            return result;
        }

        /// <summary>
        /// 跟News Web API進行兩個API之間溝通，進行公告訊息內容更新請求。
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>
        /// 透過跟自己對應的dapr sidecar跟News Web API dapr sidecar進行溝通。
        /// </remarks>
        /// <returns></returns>
        [HttpPost("UpdateNewsMessage")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> UpdateNewsMessageHandler(NewsContent data)
        {
            log.LogInformation($"收到更新公告內容請求，依據公告編號: {data.Id}");

            EventDataBase result;

            HttpResponseMessage response = new HttpResponseMessage();

            // [area]/[controller]/[template]
            string queryURL = $"{AreaControllerPath}/Update";

            response = await _httpClient.HttpResponseByPut(
                requestData: data,
                appId: AppServicesName.DaprApp_NewsName,
                requestUrl: queryURL
            );

            if (!response.IsSuccessStatusCode)
            {
                log.LogWarning("請求過程發生錯誤，事件請求已終止執行。");
                throw new Exception("The error has occurred.");
            }

            result = await response.Content.ReadFromJsonAsync<EventDataBase>();

            return result;
        }

        /// <summary>
        /// News Web API進行兩個API之間溝通，進行公告訊息內容新增請求。
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>
        /// 透過跟自己對應的dapr sidecar跟News Web API dapr sidecar進行溝通。
        /// </remarks>
        /// <returns></returns>
        [HttpPost("CreateNewsMessage")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> CreateNewsMessageHandler(NewsMessageArgument data)
        {
            log.LogInformation($"收到新增公告內容請求");

            EventDataBase result;

            HttpResponseMessage response = new HttpResponseMessage();

            // [area]/[controller]/[template]
            string queryURL = $"{AreaControllerPath}/Create";

            response = await _httpClient.HttpResponseByPost(
                requestData: data,
                appId: AppServicesName.DaprApp_NewsName,
                requestUrl: queryURL
            );

            if (!response.IsSuccessStatusCode)
            {
                log.LogWarning("請求過程發生錯誤，事件請求已終止執行。");
                throw new Exception("The error has occurred.");
            }

            result = await response.Content.ReadFromJsonAsync<EventDataBase>();

            return result;
        }

        /// <summary>
        /// News Web API進行兩個API之間溝通，進行公告訊息刪除請求。
        /// </summary>
        /// <remarks>
        /// 透過跟自己對應的dapr sidecar跟News Web API dapr sidecar進行溝通。
        /// </remarks>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteNewsMessag")]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status200OK)]
        public async Task<ResponseResult> DeleteNewsMessageHandler([FromQuery] string Id)
        {
            log.LogInformation($"收到刪除公告請求，依據公告編號: {Id}");

            ResponseResult result;
            HttpResponseMessage response = new HttpResponseMessage();

            // [area]/[controller]/[template]
            string queryURL = $"{AreaControllerPath}/Delete?Id={Id}";

            response = await _httpClient.HttpResponseByDelete(
                appId: AppServicesName.DaprApp_NewsName,
                requestUrl: queryURL
            );

            if (!response.IsSuccessStatusCode)
            {
                log.LogWarning("請求過程發生錯誤，事件請求已終止執行。");
                throw new Exception("The error has occurred.");
            }

            result = await response.Content.ReadFromJsonAsync<ResponseResult>();

            return result;
        }
    }
}
