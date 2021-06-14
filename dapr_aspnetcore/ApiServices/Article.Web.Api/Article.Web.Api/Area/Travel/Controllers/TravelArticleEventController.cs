using DaprApp.Interface.statestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Web.Api.Area.Travel.Controllers
{
    [Area("Travel")]
    [Route("[area]/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TravelArticleEventController : ControllerBase
    {
        private readonly ILogger<TravelArticleEventController> log;
        private readonly IStateStoreEvent daprStateStore;

        public TravelArticleEventController(
            ILogger<TravelArticleEventController> logger,
            IStateStoreEvent stateStoreEvent
        )
        {
            log = logger;
            daprStateStore = stateStoreEvent;
        }
    }
}
