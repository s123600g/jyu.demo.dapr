using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Web.Api.Filter
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        /// <summary>
        /// 攔截進入Action前事件。
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context) { }

        /// <summary>
        /// 攔截當Action結束後回應事件。
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 偵測當前Http Context是否有發生例外錯誤，如果有的話就要接手處理回應訊息
            if (context.Exception != null)
            {
                Exception getException = context.Exception;

                context.Result = new ObjectResult(getException.GetType().Name)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
