using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NotadogApi.Controllers
{
    /// <summary>
	/// Users API
	/// </summary>
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Get all users.
        /// </summary>  
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "user1", "user2" };
        }
    }
}