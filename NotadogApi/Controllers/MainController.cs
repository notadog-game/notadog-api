using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NotadogApi.Controllers
{
    /// <summary>
	/// Main
	/// </summary>
    [Route("/")]
    [ApiController]
    public class MainController : ControllerBase
    {
        /// <summary>
        /// Get index.
        /// </summary>  
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Notadog app API! api/v1";
        }
    }
}
