using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainWebApi.Controllers
{
    [Area("Home")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController() { }

        [HttpGet("Index")]
        public ActionResult Index()
        {
            return Ok("The Api is Running.");
        }
    }
}
