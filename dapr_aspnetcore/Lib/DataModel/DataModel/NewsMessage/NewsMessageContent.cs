using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataModel.EventBase;

namespace DataModel.NewsMessage
{
    public class NewsMessageContent : EventDataBase
    {
        /// <summary>
        /// 公告主題
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string Topic { set; get; }

        /// <summary>
        /// 公告內容
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string Content { set; get; }
    }
}
