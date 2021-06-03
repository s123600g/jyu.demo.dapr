using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Dapr
{
    /// <summary>
    /// 放置Dapr 各元件App Id名稱
    /// </summary>
    public static class DaprComponentNames
    {
        /// <summary>
        /// Dapr publish/subscribe component app id.
        /// </summary>
        public const string PubSubComponent = "pubsub";

        /// <summary>
        /// Dapr StateStore component app id.
        /// </summary>
        public const string StateStoreComponent = "statestore";
    }
}
