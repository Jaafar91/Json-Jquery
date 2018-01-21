using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyServiceApi.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("count")]
        public IHttpActionResult getCountOfUsers()
        {
            return Ok(5);
        }


    }
}
