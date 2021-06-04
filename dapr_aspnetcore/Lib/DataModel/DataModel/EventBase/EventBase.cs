using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.EventBase
{
    public class EventDataBase
    {
        /// <summary>
        /// 基礎事件紀錄核心建構子
        /// </summary>
        /// <remarks>
        /// 在建構子中會自動產生事件編號和事件產生時間。
        /// </remarks>
        public EventDataBase()
        {
            eventId = Guid.NewGuid().ToString();
            eventCreateDate = DateTime.UtcNow.ToString("yyyy-MM-d H:mm:ss");
        }

        /// <summary>
        /// 事件編號
        /// </summary>
        public string eventId { set; get; }

        /// <summary>
        /// 事件建置時間
        /// </summary>
        public string eventCreateDate { set; get; }

        /// <summary>
        /// 事件主題
        /// </summary>
        public string eventTopic { set; get; }

        /// <summary>
        /// 事件內容
        /// </summary>
        public string eventContent { set; get; }
    }
}
