using Dapr;
using dapr_aspnetcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapr_aspnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> log;

        public SampleController(
            ILogger<SampleController> logger
        )
        {
            log = logger;
        }

        // 這裡的名稱會跟Dapr設定檔'statestore.yaml'有關
        // Set state store Name
        public const string StoreName = "statestore";

        /// <summary>
        /// Gets the account information as specified by the id.
        /// 依據帳戶ID取得對應得餘額紀錄
        /// </summary>
        /// <param name="account">
        /// 依據存放在Redis內對應的Session紀錄來做資訊取得 <br/>
        /// Account information for the id from Dapr state store.
        /// </param>
        /// <returns>Account information.</returns>
        [HttpGet("{account}")]
        public ActionResult<Account> Get(
            [FromState(StoreName)] StateEntry<Account> account
        )
        {
            // 如果Redis內沒有該會員的交易紀錄，就會被這段判斷捕捉到
            if (account.Value is null)
            {
                // 這個會回傳Http Status code 404
                return this.NotFound();
            }

            return account.Value;
        }

    }
}
