using Dapr.Client;
using DaprApp.Models;
using DataModel.ArticleContent;
using DataModel.EventBase;
using DataModel.Response;
using MainWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MainWebApi.Area.Article.Controller.Food
{
    [Area("Article-Food")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class ArticleFoodHttpClientController : ControllerBase
    {
        private readonly ILogger<ArticleFoodHttpClientController> log;
        private readonly AppServicesName AppServicesName;

        public ArticleFoodHttpClientController(
            ILogger<ArticleFoodHttpClientController> logger,
            IOptionsMonitor<AppServicesName> options
        )
        {
            log = logger;
            AppServicesName = options.CurrentValue;
        }

        private const string AreaControllerPath = "/Food/FoodArticleEvent";

        [HttpGet("Query")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> QueryArticleHandler(string Id)
        {
            log.LogInformation($"收到查詢文章內容請求，依據公告編號: {Id}");

            EventDataBase result;

            using (var client = DaprClient.CreateInvokeHttpClient(appId: AppServicesName.DaprApp_ArticleName))
            {
                // [area]/[controller]/[template]
                string queryURL = $"{AreaControllerPath}/Query?Id={Id}";

                var response = await client.GetAsync(queryURL);

                if (!response.IsSuccessStatusCode)
                {
                    log.LogWarning("請求過程發生錯誤，事件請求已終止執行。");
                    throw new Exception("The error has occurred.");
                }

                result = await response.Content.ReadFromJsonAsync<EventDataBase>();
            }

            return result;
        }

        [HttpPost("Update")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> UpdateArticleHandler(ArticleArgContent data)
        {
            log.LogInformation($"收到更新文章內容請求，依據公告編號: {data.Id}");

            EventDataBase result;

            using (var client = DaprClient.CreateInvokeHttpClient(appId: AppServicesName.DaprApp_ArticleName))
            {
                // [area]/[controller]/[template]
                string queryURL = $"{AreaControllerPath}/Update";

                var response = await client.PutAsJsonAsync(queryURL, data);

                if (!response.IsSuccessStatusCode)
                {
                    log.LogWarning("請求過程發生錯誤，事件請求已終止執行。");
                    throw new Exception("The error has occurred.");
                }

                result = await response.Content.ReadFromJsonAsync<EventDataBase>();
            }

            return result;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> CreateArticleHandler(ArticleContent data)
        {
            log.LogInformation($"收到新增文章內容請求");

            EventDataBase result;

            using (var client = DaprClient.CreateInvokeHttpClient(appId: AppServicesName.DaprApp_ArticleName))
            {
                // [area]/[controller]/[template]
                string queryURL = $"{AreaControllerPath}/Create";

                var response = await client.PostAsJsonAsync(queryURL, data).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    log.LogWarning("請求過程發生錯誤，事件請求已終止執行。");
                    throw new Exception("The error has occurred.");
                }

                result = await response.Content.ReadFromJsonAsync<EventDataBase>();
            }

            return result;
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status200OK)]
        public async Task<ResponseResult> DeleteArticleHandler([FromQuery] string Id)
        {
            log.LogInformation($"收到刪除文章請求，依據公告編號: {Id}");

            ResponseResult result;

            using (var client = DaprClient.CreateInvokeHttpClient(appId: AppServicesName.DaprApp_ArticleName))
            {
                // [area]/[controller]/[template]
                string queryURL = $"{AreaControllerPath}/Delete?Id={Id}";

                var response = await client.DeleteAsync(queryURL);

                if (!response.IsSuccessStatusCode)
                {
                    log.LogWarning("請求過程發生錯誤，事件請求已終止執行。");
                    throw new Exception("The error has occurred.");
                }

                result = await response.Content.ReadFromJsonAsync<ResponseResult>();
            }

            return result;
        }
    }
}
