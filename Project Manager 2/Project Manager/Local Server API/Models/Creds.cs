using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Local_Server_API.Models
{
    public class Creds
    {
        public string username { get; set; }
        public string password { get; set; }

        public override string ToString() => username + password;
    }
}