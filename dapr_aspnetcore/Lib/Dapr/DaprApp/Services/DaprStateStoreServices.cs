using Dapr.Client;
using DaprApp.Interface.statestore;
using DataModel.EventBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DataModel.Dapr;
using static DataModel.Dapr.DaprComponentNames;
using Dapr;
using DataModel.Response;
using DataModel.NewsMessage;

namespace DaprApp.Services
{
    public class DaprStateStoreServices : IStateStoreEvent
    {
        private readonly DaprClient dapr;
        private readonly ILogger<DaprStateStoreServices> log;

        public DaprStateStoreServices(
            DaprClient daprClient,
            ILogger<DaprStateStoreServices> logger
        )
        {
            dapr = daprClient;
            log = logger;
        }

        public async Task DeleteStateEvent(string Id)
        {
            try
            {
                await dapr.DeleteStateAsync(
                    StateStoreComponent, // Dapr state store component name.
                    Id // Dapr state key name.
                );
            }
            catch (Exception ex)
            {
                log.LogInformation("The dapr state deleted failed.");
                throw ex;
            }
        }

        public async Task<EventDataBase> GetStateEvent(string Id)
        {
            EventDataBase stateData = null;

            try
            {
                StateEntry<EventDataBase> getState = await dapr.GetStateEntryAsync<EventDataBase>(
                    StateStoreComponent, // Dapr state store component name.
                    Id // Dapr state key name.
                );

                if (getState != null)
                {
                    stateData = getState.Value;
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("The dapr state query failed.");
                throw ex;
            }

            return stateData;
        }

        public async Task SaveStateEvent<Target>(Target targetData)
            where Target : EventDataBase
        {
            try
            {
                // 狀態管理 - 使用 Dapr .NET SDK
                // https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/state-management#use-the-dapr-net-sdk


                #region 非狀態鎖定(Etag)的更新方式

                await dapr.SaveStateAsync(
                    StateStoreComponent, // Dapr state store component name.
                    targetData.eventId, // Dapr state key name.
                    (dynamic)targetData
                );

                #endregion

                #region 有狀態鎖定(Etag)的更新方式

                // Why does GetStateAndETagAsync exist
                // https://github.com/dapr/dotnet-sdk/issues/525

                // 透過GetStateAndETagAsync來取得目前的state紀錄實體與在dapr中Etag
                // Etag概念是每個state紀錄都會有一個鎖定狀態碼概念，當state紀錄有做更動時，此Etag內的的值會做改變
                // 採用先到先獲勝的概念(last-write-wins)
                // 意謂者如果同一時間有A、B、C在使用同一筆state紀錄，三者經過查詢取的的state紀錄和Etag都是相同的
                // 當三者今天都要更新同一筆state紀錄時，假設B先更新完成了，在Dapr中的state紀錄Etag內的值會被更新
                // A、C兩個也要更新此筆state紀錄時，執行會發生儲存失敗，因為他們手中的Etag的值已經跟Dape的state紀錄Etag不一樣
                // 如果今天兩者要更新此筆state紀錄，就要再重新查詢取得該state紀錄與Etag，最後再看兩者誰又最先儲存更新完成。

                //var (getState, etag) = await dapr.GetStateAndETagAsync<Target>(
                //    StateStoreComponent, // Dapr state store component name.
                //    targetData.eventId // Dapr state key name.
                //);

                //if (getState != null)
                //{
                //    bool getSaveState = await dapr.TrySaveStateAsync(
                //       StateStoreComponent, // Dapr state store component name.
                //       targetData.eventId, // Dapr state key name.
                //       (dynamic)targetData, // Dapr state value.
                //       etag // Dapr state etag.
                //    );

                //    if (!getSaveState)
                //    {
                //        throw new Exception("The dapr state can't save.");
                //    }
                //}

                #endregion

            }
            catch (Exception ex)
            {
                log.LogInformation("The dapr state saved failed.");

                throw ex;
            }
        }

        public async Task UpdateStateEvent<Target>(Target targetData)
            where Target : EventDataBase
        {
            try
            {
                StateEntry<Target> getState = await dapr.GetStateEntryAsync<Target>(
                    StateStoreComponent, // Dapr state store component name.
                    targetData.eventId // Dapr state key name.
                );

                if (getState != null)
                {
                    getState.Value.eventTopic = targetData.eventTopic;
                    getState.Value.eventContent = targetData.eventContent;

                    await getState.SaveAsync();
                }
                else
                {
                    throw new KeyNotFoundException("The dapr state wasn't existed.");
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("The dapr state updated failed.");

                throw ex;
            }
        }
    }
}
