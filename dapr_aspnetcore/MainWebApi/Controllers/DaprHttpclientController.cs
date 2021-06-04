using Dapr.Client;
using DataModel.EventBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using static DataModel.Dapr.DaprAppIdName;

namespace MainWebApi.Controllers
{
    [Area("Dpar_HttpClient")]
    [Route("[area]/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class DaprHttpclientController : ControllerBase
    {
        public DaprHttpclientController()
        {
        }

        /// <summary>
        /// Test api
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryNewsMessage")]
        [ProducesResponseType(typeof(EventDataBase), StatusCodes.Status400BadRequest)]
        public async Task<EventDataBase> QueryNewsMessageHandler(string Id)
        {
            var client = DaprClient.CreateInvokeHttpClient(appId: dapr_aspnetcoreAppName);

            // [area]/[controller]/[action]
            string queryURL = $"/News/News/Query?Id={Id}";

            //var response = await client.PostAsJsonAsync(queryURL, news_data);
            var response = await client.GetAsync(queryURL);

            var news = await response.Content.ReadFromJsonAsync<EventDataBase>();

            return news;
        }
    }
}
