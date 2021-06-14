using DaprApp.Interface.statestore;
using DataModel.EventBase;
using DataModel.NewsMessage;
using DataModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Web.Api.Area.System.Controllers
{
    [Route("[area]/[controller]")]
    [Area("System")]
    [Produces("application/json")]
    [ApiController]
    public class SystemNewsEventController : ControllerBase
    {
        private readonly ILogger<SystemNewsEventController> log;
        private readonly IStateStoreEvent daprStateStore;

        public SystemNewsEventController(
            ILogger<SystemNewsEventController> logger,
            IStateStoreEvent stateStoreEvent
        )
        {
            log = logger;
            daprStateStore = stateStoreEvent;
        }

        /// <summary>
        /// 查詢公告訊息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("Query")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> Query([FromQuery] string Id)
        {
            EventDataBase queryData = null;

            log.LogInformation($"收到查詢活動公告編號:{Id}內容請求.");

            queryData = await daprStateStore.GetStateEvent(Id);

            return queryData;
        }

        /// <summary>
        /// 更新公告訊息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> Update(Message data)
        {
            log.LogInformation($"收到更新公告編號:{data.Id}內容請求.");

            EventDataBase queryData = await daprStateStore.GetStateEvent(data.Id);

            queryData.eventTopic = data.Topic;
            queryData.eventContent = data.Content;

            await daprStateStore.UpdateStateEvent(queryData);

            return queryData;
        }

        /// <summary>
        /// 新增公告訊息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status200OK)]
        public async Task<EventDataBase> Create(NewsMessageArgument data)
        {
            log.LogInformation($"收到新增活動公告請求.");

            EventDataBase newData = new EventDataBase();
            newData.eventTopic = data.Topic;
            newData.eventContent = data.Content;

            await daprStateStore.SaveStateEvent(newData);

            return newData;
        }

        /// <summary>
        /// 刪除公告訊息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status200OK)]
        public async Task<ResponseResult> Delete([FromQuery] string Id)
        {
            log.LogInformation($"收到刪除活動公告編號:{Id}內容請求.");

            ResponseResult result = new ResponseResult();

            EventDataBase getData = await daprStateStore.GetStateEvent(Id);

            if (getData != null)
            {

                await daprStateStore.DeleteStateEvent(Id);

                result.RunStatus = true;
                result.Msg = "Done.";
            }
            else
            {
                result.Msg = "該筆公告紀錄不存在";
            }

            return result;
        }
    }
}
