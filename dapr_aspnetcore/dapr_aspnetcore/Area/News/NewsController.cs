using DaprApp.Interface.statestore;
using DataModel.EventBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapr_aspnetcore.Area.News
{
    [Route("[area]/[controller]")]
    [Area("News")]
    [Produces("application/json")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> log;
        private readonly IStateStoreEvent daprStateStore;

        public NewsController(
            ILogger<NewsController> logger,
            IStateStoreEvent stateStoreEvent
        )
        {
            log = logger;
            daprStateStore = stateStoreEvent;
        }

        [HttpGet("Query")]
        public async Task<EventDataBase> Query([FromQuery] string Id)
        {
            EventDataBase queryData = null;

            log.LogInformation($"收到查詢公告編號:{Id}內容請求.");

            queryData = await daprStateStore.GetStateEvent(Id);

            return queryData;
        }
    }
}
