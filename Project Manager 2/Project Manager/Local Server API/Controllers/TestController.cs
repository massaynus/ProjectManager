using DataAccess.Models;
using Local_Server_API.Models;
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
        User user = new User();
        bool CtorCalled = false;
        
        public TestController()
        {
            CtorCalled = true;
        }

        public Object GetValue()
        {
            return new { Data = CtorCalled };
        }
    }
}
