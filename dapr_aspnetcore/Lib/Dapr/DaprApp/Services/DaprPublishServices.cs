using Dapr.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using DaprApp.Interface.pubsub;
using System.Threading.Tasks;
using DataModel.Response;
using DataModel.EventBase;
using static DataModel.Dapr.DaprComponentNames;

namespace DaprApp.Services
{
    public class DaprPublishServices : IPubSubEvent
    {
        private readonly ILogger<DaprPublishServices> log;
        private readonly DaprClient daprClientInstance;

        public DaprPublishServices(
            ILogger<DaprPublishServices> logger,
            DaprClient daprClient
        )
        {
            log = logger;
            daprClientInstance = daprClient;
        }

        public async Task PublishEvent<Target>(
            Target eventData
        )
            where Target : EventDataBase
        {
            try
            {
                // 發佈訊息至queues
                await daprClientInstance.PublishEventAsync(
                    PubSubComponent, // pub/sub component name.
                    eventData.eventTopic, // publish topic name.
                    (dynamic)eventData// topic content.
                );

            }
            catch (Exception ex)
            {
                log.LogInformation($"The Dapr client published error. Event Id {""} \n{ex.Message}\n{ex.StackTrace}");

                throw ex;
            }
        }
    }
}
