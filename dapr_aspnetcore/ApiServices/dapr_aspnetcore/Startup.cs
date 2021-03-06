using DaprApp.Interface.pubsub;
using DaprApp.Interface.statestore;
using DaprApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace dapr_aspnetcore
{
    // 使用 Dapr .NET SDK
    // https://docs.microsoft.com/zh-tw/dotnet/architecture/dapr-for-net-developers/publish-subscribe#use-the-dapr-net-sdk

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        // 設置Reponse內JSON key命名規則使用PascalCase而不是使用預設camelCase(駝峰式命名)
                        options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    })
                    .AddDapr() // 註冊Dapr Services入App Pipline
            ;

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(SWGENOptions =>
            {
                #region 設置Swagger頁面文件內容，放置API有關簡介資訊

                SWGENOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    // 文件標題
                    Title = "Dapr_AspNetCore API",

                    // 文件敘述
                    Description = "A simple example ASP.NET Core Web API",

                    // 服務條款，這裡必須是網址
                    TermsOfService = new Uri("https://example.com/terms"),

                    // 網站連結
                    Contact = new OpenApiContact
                    {
                        Name = "dapr_aspnetcore",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },

                    // 授權資訊
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                #endregion

                // XML 註解
                // https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio#xml-comments
                // 這區塊是根據在程式內進行XML格式註解，生成一份對應參照XML文件
                // 並且在Swagger UI介面中每個API項目名稱後面會帶入對應的XML說明內容敘述
                #region 配置XML註解說明

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // 指定XML檔案名稱以專案名稱來命名
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); // 指定放在專案目錄位置下
                SWGENOptions.IncludeXmlComments(xmlPath);

                #endregion
            });

            // 補齊.net core字元編碼類型，避免出現亂碼現象
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            // 註冊Dapr PubSubEvent Services.
            services.AddTransient<IPubSubEvent, DaprPublishServices>();

            // 註冊Dapr StateStoreEvent Services.
            services.AddTransient<IStateStoreEvent, DaprStateStoreServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // https://github.com/dapr/dapr/issues/1242
            // 如果加入這一段強制轉HTTPS導向，會導致Dapr針對訂閱事件對應的Handle Api呼叫發生問題
            // 當發生Publish Event之後，Dapr會呼叫有訂閱此Event的處理Action
            // 當有加入下面這段強轉Https時候，一開始Dapr會透過80 Port呼叫有訂閱的Action
            // Middleware接收到請求之後，會回應強制Htpps 狀態碼307回應，但是Dapr無法處理此狀態事件
            // 最終會導致發生有訂閱此事件的Actioon沒有任何作為情況。
            //app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(SWOptions =>
            {
                // 這是是針對Swagger JSON檔案路由樣式配置，請注意必須要包含'{documentName}'
                // 如果有設置此項目，必須要去更新再app.UseSwaggerUI內'SwaggerEndpoint'裡面路由樣式
                // 如果沒設置此項目，預設路由為 /swagger/{documentName}/swagger.json
                SWOptions.RouteTemplate = "/api-docs/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(SWUIOptions =>
            {
                // JSON預設路由為/swagger/v1/swagger.json
                SWUIOptions.SwaggerEndpoint(
                    "/api-docs/v1/swagger.json",
                    "dap_aspnetcore API V1" // 這裡會跟介面右上方API選項內文字有關聯
                );

                // 加入此段將預設根目錄設置為Swagger UI頁面
                SWUIOptions.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseCloudEvents();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // 加入Dapr在Pulish/Subscribe Endpoint Handler
                // 讓屬於Dapr處理的pub/sub請求導向到Dapr自己的runtime去處理
                endpoints.MapSubscribeHandler();

                endpoints.MapControllers();

                // 設定Api路由樣式匹配規則
                endpoints.MapControllerRoute(
                    name: "ApiArea",
                    pattern: "{area:exists}/{controller}/{action}"
                );
            });
        }
    }
}
