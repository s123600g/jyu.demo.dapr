using Dapr;
using Dapr.Client;
using dapr_aspnetcore.Models;
using DaprApp.Interface.pubsub;
using DataModel.NewsMessage;
using DataModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataModel.Dapr.DaprComponentNames;
using static DataModel.Dapr.DaprEventNames;

namespace dapr_aspnetcore.Controllers
{
    [Route("[area]/[controller]")]
    [Area("NewsMsg")]
    [ApiController]
    [Produces("application/json")]
    public class NewsMsgController : ControllerBase
    {
        private readonly ILogger<NewsMsgController> log;
        private readonly DaprClient dapr;
        private readonly IPubSubEvent daprPubSub;

        public NewsMsgController(
            ILogger<NewsMsgController> logger,
            DaprClient daprClient,
            IPubSubEvent pubSubEvent
        )
        {
            log = logger;
            dapr = daprClient;
            daprPubSub = pubSubEvent;
        }

        /// <summary>
        /// 發佈新公告訊息柱列。
        /// </summary>
        /// <remarks>
        /// 這裡會將發佈訊息實體丟至rabbitmq，丟完之後Dapr會去呼叫誰有去訂閱此主題，然後去呼叫那一支API。
        /// </remarks>
        /// <param name="message">公告內容實體</param>
        /// <response code="200">代表訊息已被發佈處理。</response>
        /// <response code="404">代表訊息參數錯誤。</response>
        /// <returns></returns>
        [HttpPost("PublishNewMessageEvent")]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseResult>> PublishNewMessageHandler(NewsMessageArgument message)
        {
            ResponseResult result = new ResponseResult();

            try
            {
                log.LogInformation("收到新公告發佈請求.");

                if (message == null)
                {
                    log.LogWarning("發佈公告內容為空值.");
                    return NotFound("公告內容錯誤.");
                }

                #region 封裝發佈訊息

                NewsMessageContent newsMessage = new NewsMessageContent();
                newsMessage.Topic = message.Topic;
                newsMessage.Content = message.Content;
                newsMessage.eventTopic = newMessageTopicEventName;

                #endregion

                #region 執行發佈服務

                await daprPubSub.PublishEvent(newsMessage).ConfigureAwait(false);

                #endregion

                log.LogInformation($"已發佈新公告，Id: {newsMessage.eventId}，Topic: {newsMessage.Topic}");

                result.RunStatus = true;
                result.Msg = newsMessage.eventId;
            }
            catch (Exception ex)
            {
                result.Msg = "發佈過程發生問題。";
                log.LogInformation($"{result.Msg}\n{ex.StackTrace}");
            }

            return result;
        }

        /// <summary>
        /// 訂閱是否有新公告訊息柱列，依據主題名稱來進行訂閱。
        /// </summary>
        /// <remarks>
        /// 如果該主題內容有更新，那這隻API變會被Dapr給呼叫，收到Dapr呼叫後會進行更新存在Redis內對應的狀態內容。
        /// </remarks>
        /// <param name="message">接收到發佈的主題內容</param>
        /// <returns></returns>
        [Topic(PubSubComponent, newMessageTopicEventName)]
        [HttpPost("SubscribeNewMessageEvent")]
        public async Task<ActionResult> SubscribeNewMessageEventHandler(NewsMessageContent message)
        {
            log.LogInformation("收到有新公告發佈訊息，執行新增該公告狀態紀錄.");

            // 將發佈的新公告內容更新至Redis內
            await dapr.SaveStateAsync(
                StateStoreComponent, // Dapr state store component name.
                message.eventId, // 這裡我們依據各公告的Id來進行狀態紀錄Key
                message //  Key內容
            );

            return Ok();
        }

        /// <summary>
        /// 刪除公告狀態。
        /// </summary>
        /// <remarks>
        /// 依據公告Id去查詢Dapr state store內紀錄，如果該紀錄有存在的話便刪除在Redis內該筆公告紀錄。
        /// </remarks>
        /// <param name="Id">公告訊息紀錄編號</param>
        /// <returns></returns>
        [HttpDelete("DeleteNewsMessageState")]
        public async Task<ActionResult> DeleteNewsMessageHandler(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                log.LogWarning("未輸入公告編號.");
                return NotFound("公告編號錯誤.");
            }

            log.LogInformation($"收到刪除公告編號: {Id} 內容請求.");

            // 查詢公告State紀錄實體
            StateEntry<Message> getState = await dapr.GetStateEntryAsync<Message>(
                StateStoreComponent, // Dapr state store component name.
                Id // 公告State紀錄Key都是以自己Id來命名
            );

            if (getState == null)
            {
                log.LogWarning("未有該筆公告State紀錄.");
                return NotFound("該筆公告不存在.");
            }

            // 執行刪除該公告狀態紀錄
            await getState.DeleteAsync();

            log.LogInformation($"公告編號: {Id} 已刪除.");

            return Ok("該筆公告已刪除.");
        }

        /// <summary>
        /// 更新共告狀態。
        /// </summary>
        /// <remarks>
        /// 依據公告Id去查詢Dapr state store內紀錄，如果該紀錄有存在的話便能夠更新在Redis內該筆公告紀錄。
        /// </remarks>
        /// <param name="message">公告訊息內容</param>
        /// <returns></returns>
        [HttpPost("UpdateNewsMessageState")]
        public async Task<ActionResult> UpdateNewsMessageHandler(Message message)
        {
            // 查詢公告State紀錄實體
            StateEntry<Message> getState = await dapr.GetStateEntryAsync<Message>(
                StateStoreComponent, // Dapr state store component name.
                message.Id // 公告State紀錄Key都是以自己Id來命名
            );

            if (getState.Value == null)
            {
                log.LogWarning("未有該筆公告State紀錄.");
                return NotFound("該筆公告紀錄不存在.");
            }

            // 執行更新該公告內容
            getState.Value.Topic = message.Topic;
            getState.Value.Content = message.Content;

            // 將異動內容更新至State
            await getState.SaveAsync();

            log.LogInformation($"已更新公告編號:{message.Id} 內容.");

            return Ok("公告內容已更新.");
        }

        /// <summary>
        /// 取得公告狀態。
        /// </summary>
        /// <remarks>
        /// 依據公告Id去查詢Dapr state store內紀錄，如果該紀錄有存在的話便能夠在Redis內查詢的到該筆公告的Key-Value紀錄。
        /// </remarks>
        /// <param name="Id">公告紀錄State Id</param>
        /// <returns></returns>
        [HttpGet("QueryNewsMessageState")]
        public async Task<ActionResult<Message>> QueryNewsMessageHandler(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                log.LogWarning("未輸入公告編號.");
                return NotFound("公告編號錯誤.");
            }

            log.LogInformation($"收到查詢公告編號: {Id} 內容請求.");

            // 查詢公告State紀錄實體
            StateEntry<Message> getState = await dapr.GetStateEntryAsync<Message>(
                StateStoreComponent, // Dapr state store component name.
                Id // 公告State紀錄Key都是以自己Id來命名
            );

            if (getState.Value == null)
            {
                log.LogWarning("未有該筆公告State紀錄.");
                return NotFound("該筆公告紀錄不存在.");
            }

            return getState.Value;
        }
    }
}
