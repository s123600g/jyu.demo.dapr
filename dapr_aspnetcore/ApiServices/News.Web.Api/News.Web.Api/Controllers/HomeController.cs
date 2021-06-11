using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Web.Api.Controllers
{
    [Route("[area]/[controller]")]
    [Area("Test")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController() { }

        [HttpGet("home")]
        public ActionResult home()
        {
            return Ok("The web api is running.");
        }
    }
}
