using Dapr.Client;
using DataModel.EventBase;
using DataModel.NewsMessage;
using DataModel.Response;
using MainWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using static DataModel.Dapr.DaprAppIdName;

namespace MainWebApi.Controllers
{
    [Area("Test")]
    [Route("[area]/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class HttpClientController : ControllerBase
    {
        private readonly ILogger<HttpClientController> log;
        public HttpClientController(ILogger<HttpClientController> logger)
        {
            log = logger;
        }

        /// <summary>
        /// 跟dapr_asprnetcore Web API進行兩個API之間溝通，進行公告訊息查詢請求。
        /// </summary>
        /// <remarks>
        /// 透過跟自己對應的dapr sidecar跟dapr_aspnetcore dapr sidecar進行溝通。
        /// </remarks>
        /// <param name="Id">公告編號</param>
        /// <returns></returns>
        [HttpGet("QueryNewsMessage")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> QueryNewsMessageHandler(string Id)
        {
            log.LogInformation($"收到查詢公告內容請求，依據公告編號: {Id}");

            var client = DaprClient.CreateInvokeHttpClient(appId: dapr_aspnetcoreAppName);

            // [area]/[controller]/[template]
            string queryURL = $"/News/News/Query?Id={Id}";

            var response = await client.GetAsync(queryURL);

            var news = await response.Content.ReadFromJsonAsync<EventDataBase>();

            return news;
        }

        /// <summary>
        /// 跟dapr_asprnetcore Web API進行兩個API之間溝通，進行公告訊息內容更新請求。
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>
        /// 透過跟自己對應的dapr sidecar跟dapr_aspnetcore dapr sidecar進行溝通。
        /// </remarks>
        /// <returns></returns>
        [HttpPost("UpdateNewsMessage")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> UpdateNewsMessageHandler(NewsContent data)
        {
            log.LogInformation($"收到更新公告內容請求，依據公告編號: {data.Id}");

            var client = DaprClient.CreateInvokeHttpClient(appId: dapr_aspnetcoreAppName);

            // [area]/[controller]/[template]
            string queryURL = $"/News/News/Update";

            var response = await client.PostAsJsonAsync(queryURL, data);

            var news = await response.Content.ReadFromJsonAsync<EventDataBase>();

            return news;
        }

        /// <summary>
        /// 跟dapr_asprnetcore Web API進行兩個API之間溝通，進行公告訊息內容新增請求。
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>
        /// 透過跟自己對應的dapr sidecar跟dapr_aspnetcore dapr sidecar進行溝通。
        /// </remarks>
        /// <returns></returns>
        [HttpPost("CreateNewsMessage")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> CreateNewsMessageHandler(NewsMessageArgument data)
        {
            log.LogInformation($"收到新增公告內容請求");

            var client = DaprClient.CreateInvokeHttpClient(appId: dapr_aspnetcoreAppName);

            // [area]/[controller]/[template]
            string queryURL = $"/News/News/Create";

            var response = await client.PostAsJsonAsync(queryURL, data);

            var news = await response.Content.ReadFromJsonAsync<EventDataBase>();

            return news;
        }

        /// <summary>
        /// 跟dapr_asprnetcore Web API進行兩個API之間溝通，進行公告訊息刪除請求。
        /// </summary>
        /// <remarks>
        /// 透過跟自己對應的dapr sidecar跟dapr_aspnetcore dapr sidecar進行溝通。
        /// </remarks>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteNewsMessag")]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status200OK)]
        public async Task<ResponseResult> DeleteNewsMessageHandler([FromQuery] string Id)
        {
            log.LogInformation($"收到刪除公告內容請求，依據公告編號: {Id}");

            var client = DaprClient.CreateInvokeHttpClient(appId: dapr_aspnetcoreAppName);

            // [area]/[controller]/[template]
            string queryURL = $"/News/News/Create?Id={Id}";

            var response = await client.DeleteAsync(queryURL);

            var result = await response.Content.ReadFromJsonAsync<ResponseResult>();

            return result;
        }
    }
}
