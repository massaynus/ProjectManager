using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows_CLient.Models;

namespace Windows_CLient
{
    public static class APIClient
    {
        public static readonly string API_HOST = ConfigurationManager.AppSettings["API_HOST"] ?? string.Empty;
        public static HttpClient client;
        public static User User { get; set; }

        public static void initClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         
            client.BaseAddress = new Uri(API_HOST);
        }
    }
}
