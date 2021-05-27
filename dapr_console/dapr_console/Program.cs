using Dapr.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Net.Http.Json;

namespace dapr_console
{
    class Program
    {
        // Dapr 入門
        //https://docs.microsoft.com/zh-tw/dotnet/architecture/dapr-for-net-developers/getting-started
        // 使用 Dapr .NET SDK
        // https://docs.microsoft.com/zh-tw/dotnet/architecture/dapr-for-net-developers/service-invocation#use-the-dapr-net-sdk

        // https://docs.dapr.io/reference/cli/dapr-run/
        // dapr run [flags] [command]
        // 此Sample所使用的Dapr指令為
        // --> dapr run --app-id DaprCounter --dapr-http-port 3500 --dapr-grpc-port 50001
        // 範例中後面會帶'dotnet run'，這是屬於[command]區塊，意思是當dapr啟動完畢後，執行當前.net app
        // 前提是你的終端機位置要再專案目錄底下，因為它會抓取當前位置底下去找專案檔執行
        // 因應每個電腦本機環境預設跑起來會有機會不是Dapr在Http/grpc預設Port
        // Http 預設Port 3500 ，gRPC預設Port 50001
        // 如果再程式建立連接實體上，未有任何設置相關Endpoint，就會跑預設Port

        private static IConfiguration Appsettings;
        private static Logger log;

        static async Task Main(string[] args)
        {
            // 載入Appsettings
            Appsettings = AppInitialize.InitAppsettings();
            // 載入NLog
            log = AppInitialize.InitNlog(Appsettings);

            // Dapr Component Name
            // 這裡指的是要指定哪一個Dapr component作為來源
            // 可以想像成每一個Component都是一個獨立的App概念，內部會有很多State紀錄.
            // 在此範例中使用的Component被命名稱'statestore'
            // 是吃預設Dapr在系統存放的位置底下'components/'會有一個statestore.yaml
            // 此YAML檔案就是此Sample所使用的Dapr Component配置來源
            // 根據內部'metadata'階層name來設值命名
            // 這裡的名稱是給程式呼叫更新、查詢、刪除相關API再用
            // 如果你有去看Redis內每一個Key前面都會有一個'前置詞'
            // 這個前置詞是app-id，格式 --> 前置詞||Key
            const string storeName = "statestore";

            // 存放在該Component內紀錄Key Name，依據此Name取得對應的Key內容(Value)用途
            // 這裡使用的是Redis來存放各Component內Key-Value Satte紀錄
            // 這裡Sample是取得計數器內的數值 --> {counter: 0}
            const string key = "counter";

            try
            {
                var daprClient = new DaprClientBuilder().Build();

                // 查詢在Component內Key為counter的紀錄內容數值
                var counter = await daprClient.GetStateAsync<int>(
                    storeName,
                    key
                );

                log.Info($"目前計數器: {counter}");

                counter += 1;

                log.Info("更新計數器數值，數值加一");

                // 將Key內的Value更新
                await daprClient.SaveStateAsync(
                    storeName,
                    key,
                    counter
                );

                // 查詢在Component內Key為counter的紀錄內容數值
                counter = await daprClient.GetStateAsync<int>(
                    storeName,
                    key
                );

                log.Info($"更新後計算器數值: {counter}");
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
