using Article.Web.Api.Models;
using DaprApp.Interface.statestore;
using DataModel.ArticleContent;
using DataModel.EventBase;
using DataModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Web.Api.Area.Travel.Controllers
{
    [Area("Travel")]
    [Route("[area]/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TravelArticleEventController : ControllerBase
    {
        private readonly ILogger<TravelArticleEventController> log;
        private readonly IStateStoreEvent daprStateStore;

        public TravelArticleEventController(
            ILogger<TravelArticleEventController> logger,
            IStateStoreEvent stateStoreEvent
        )
        {
            log = logger;
            daprStateStore = stateStoreEvent;
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(ArticleContent data)
        {
            log.LogInformation($"收到新增文章請求.");

            EventDataBase newData = new EventDataBase();
            newData.eventTopic = data.Title;
            newData.eventContent = data.Description;

            await daprStateStore.SaveStateEvent(newData);

            return Ok(newData);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update(Message data)
        {
            log.LogInformation($"收到更新文章編號:{data.Id}內容請求.");

            EventDataBase queryData = await daprStateStore.GetStateEvent(data.Id);

            queryData.eventTopic = data.Title;
            queryData.eventContent = data.Description;

            await daprStateStore.UpdateStateEvent(queryData);

            return Ok(queryData);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(string id)
        {
            log.LogInformation($"收到刪除文章編號:{id}內容請求.");

            ResponseResult result = new ResponseResult();

            EventDataBase getData = await daprStateStore.GetStateEvent(id);

            if (getData != null)
            {
                await daprStateStore.DeleteStateEvent(id);

                result.RunStatus = true;
                result.Msg = "Done.";
            }
            else
            {
                result.Msg = "該筆紀錄不存在";
            }

            return Ok(result);
        }

        [HttpGet("Query")]
        public async Task<ActionResult> Query(string id)
        {
            log.LogInformation($"收到查詢文章編號:{id}內容請求.");

            EventDataBase queryData = await daprStateStore.GetStateEvent(id) ?? new EventDataBase();

            return Ok(queryData);
        }

    }
}
