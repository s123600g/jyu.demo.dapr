using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Response
{
    /// <summary>
    /// 封裝服務功能回應的結果
    /// </summary>
    public class ResponseResult
    {
        public ResponseResult()
        {
            // 預設給予執行狀態為False
            this.RunStatus = false;
        }

        /// <summary>
        /// 執行結果狀態
        /// </summary>
        public bool RunStatus { set; get; }

        /// <summary>
        /// 回應訊息
        /// </summary>
        public string Msg { set; get; }
    }
}
