﻿using System;
using System.Collections.Generic;
using System.Text;
using DataModel.Response;
using DataModel.NewsMessage;
using System.Threading.Tasks;
using DataModel.EventBase;

namespace DaprApp.Interface.pubsub
{
    public interface IPubSubEvent
    {
        /// <summary>
        /// 呼叫Dapr pubsub進行發佈訊息動作。
        /// </summary>
        /// <remarks>
        /// 在此方法中會約束所有帶入的參數都必須要去繼承EventDataBase類別。
        /// </remarks>
        /// <typeparam name="Target">資料來源物件類型</typeparam>
        Task PublishEvent<Target>(Target targetData) where Target : EventDataBase;
    }
}
