using DaprApp.Interface.statestore;
using DataModel.EventBase;
using DataModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Web.Api.Area.Food.Controllers
{
    [Area("Food")]
    [Route("[area]/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class FoodArticleEventController : ControllerBase
    {
        private readonly ILogger<FoodArticleEventController> log;
        private readonly IStateStoreEvent daprStateStore;

        public FoodArticleEventController(
            ILogger<FoodArticleEventController> logger,
            IStateStoreEvent stateStoreEvent
        )
        {
            log = logger;
            daprStateStore = stateStoreEvent;
        }
    }
}
