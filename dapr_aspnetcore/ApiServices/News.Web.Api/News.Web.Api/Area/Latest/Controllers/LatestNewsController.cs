using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Web.Api.Area.Latest.Controllers
{
    [Route("[area]/[controller]")]
    [Area("Latest")]
    [ApiController]
    public class LatestNewsController : ControllerBase
    {
        public LatestNewsController() { }
    }
}
