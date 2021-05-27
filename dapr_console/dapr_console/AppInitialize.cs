using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace dapr_console
{
    class AppInitialize
    {

        // 設置settings json檔案名稱
        private const string settingsFile = "appsettings.json";

        /// <summary>
        /// 初始化載入Appsettings配置
        /// </summary>
        public static IConfiguration InitAppsettings()
        {
            IConfiguration appSettings = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile, optional: true, reloadOnChange: false)
                .Build();

            return appSettings;
        }

        /// <summary>
        /// 初始化載入NLog模組
        /// </summary>
        /// <param name="appSettings">Appsettings配置物件</param>
        public static Logger InitNlog(IConfiguration appSettings)
        {
            // NLog configuration with appsettings.json
            // https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json
            // 從組態設定檔載入NLog設定
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(appSettings.GetSection("NLog"));
            Logger logger = LogManager.GetCurrentClassLogger();

            return logger;
        }

    }
}
