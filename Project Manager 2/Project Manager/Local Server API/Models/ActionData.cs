using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Local_Server_API.Models
{
    public class ActionData
    {
        public string FromBody { get; set; }
        public List<string[]> FromURI { get; set; }
        public string FromRoute { get; set; }
    }
}