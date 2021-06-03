using DataModel.EventBase;
using DataModel.NewsMessage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaprApp.Interface.statestore
{
    public interface IStateStoreEvent
    {
        /// <summary>
        /// 呼叫Dapr StateStore狀態紀錄取得。
        /// </summary>
        /// <remarks>
        /// 在此方法中會約束所有帶入的參數都必須要去繼承EventDataBase類別。
        /// </remarks>
        /// <typeparam name="Target">資料來源物件類型</typeparam>
        /// <returns>
        /// 會回傳依據指定資料型態物件結果
        /// </returns>
        Task<Target> GetStateEvent<Target>(Target targetData) where Target : EventDataBase;

        /// <summary>
        /// 儲存紀錄至Dapr StateStore狀態。
        /// </summary>
        /// <remarks>
        /// 在此方法中會約束所有帶入的參數都必須要去繼承EventDataBase類別。
        /// </remarks>
        /// <typeparam name="Target">資料來源物件類型</typeparam>
        Task SaveStateEvent<Target>(Target targetData) where Target : EventDataBase;

        /// <summary>
        /// 呼叫Dapr StateStore刪除指定狀態紀錄。
        /// </summary>
        /// <remarks>
        /// 在此方法中會約束所有帶入的參數都必須要去繼承EventDataBase類別。
        /// </remarks>
        /// <typeparam name="Target">資料來源物件類型</typeparam>
        Task DeleteStateEvent<Target>(Target targetData) where Target : EventDataBase;

        /// <summary>
        /// 呼叫Dapr StateStore更新指定狀態紀錄。
        /// </summary>
        /// <remarks>
        /// 在此方法中會約束所有帶入的參數都必須要去繼承EventDataBase類別。
        /// </remarks>
        /// <typeparam name="Target">資料來源物件類型</typeparam>
        Task UpdateStateEvent<Target>(Target targetData) where Target : EventDataBase;
    }
}
