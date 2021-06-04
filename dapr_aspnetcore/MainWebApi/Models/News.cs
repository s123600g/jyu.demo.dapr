using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainWebApi.Models
{
    public class News
    {
        /// <summary>
        /// 公告Id
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// 公告主題
        /// </summary>
        [Required]
        public string Topic { set; get; }

        /// <summary>
        /// 公告內容
        /// </summary>
        [Required]
        public string Content { set; get; }
    }
}
