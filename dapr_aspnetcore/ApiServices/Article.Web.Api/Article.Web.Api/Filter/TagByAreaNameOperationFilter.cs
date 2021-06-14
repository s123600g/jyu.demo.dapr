using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Web.Api.Filter
{
    // .NET Core 2.1 Swashbuckle - group controllers by area
    // https://stackoverflow.com/a/67887902
    /// <summary>
    /// Swageer area tag命名過濾器
    /// </summary>
    /// <remarks>
    /// 在Swagger執行啟動時
    /// 1. 每一個Controller都會逐一去過濾內容。 <br/>
    /// 2. 逐一讀取Controller內每個Action出來。 <br/>
    /// 3. 將每個Action分配至指定的Tag群組去，Tag來源依據所屬的Controller內屬性設定。  <br/>
    /// 
    /// 有可能是Controller Name、Area、自訂群組名稱等等，在這裡是判斷是否有Area，如果沒有就以Controller作為分群依據。  <br/>
    /// 
    /// GitHub: <br/>
    /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/src/Swashbuckle.AspNetCore.SwaggerGen/SwaggerGenerator/IOperationFilter.cs
    /// </remarks>
    public class TagByAreaNameOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 實作項目過濾器套用處理方法
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context
        )
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                // 取得當前Controller的Area屬性內容名稱
                var areaName = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AreaAttribute), true)
                    .Cast<AreaAttribute>().FirstOrDefault();

                // 判斷Controller是否有設置Area，如果有就設定Swagger內Tag分類以Area作為代表
                // 反之則用Controller名稱作為Tag代表
                if (areaName != null)
                {
                    operation.Tags = new List<OpenApiTag> { new OpenApiTag { Name = areaName.RouteValue } };
                }
                else
                {
                    operation.Tags = new List<OpenApiTag> { new OpenApiTag { Name = controllerActionDescriptor.ControllerName } };
                }
            }
        }
    }
}
