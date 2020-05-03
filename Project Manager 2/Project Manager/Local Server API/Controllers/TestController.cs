using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Local_Server_API.Controllers
{
    public class TestController : ApiController
    {
        public Object GetValue()
        {
            return Request.Headers.Authorization;
        }
    }
}
