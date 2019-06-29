using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NotadogApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class MainController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Notadog app API";
        }
    }
}
