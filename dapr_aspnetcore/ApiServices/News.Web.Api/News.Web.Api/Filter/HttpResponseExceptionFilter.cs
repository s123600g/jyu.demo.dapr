using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Web.Api.Filter
{
    /// <summary>
    /// 過濾Api Action執行錯誤狀態
    /// </summary>
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

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
