using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Web.Api.Area.System.Controllers
{
    [Route("api/[controller]")]
    [Area("System")]
    [ApiController]
    public class SystemNewsController : ControllerBase
    {
        public SystemNewsController() { }
    }
}
