using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainWebApi.Models
{
    public class ArticleArgContent
    {
        public string Id { set; get; }

        [Required]
        public string Title { set; get; }

        [Required]
        public string Description { set; get; }
    }
}
