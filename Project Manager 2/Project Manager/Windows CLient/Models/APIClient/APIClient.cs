using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TT = System.Threading.Tasks;
using Windows_CLient.Models;
using System.Windows;
using System.Windows.Controls;

namespace Windows_CLient
{
    public static class APIClient
    {
        public static readonly string API_HOST = ConfigurationManager.AppSettings["API_HOST"] ?? string.Empty;
        public static HttpClient client;
        public static User User { get; set; }

        public static StringContent GetStringContent(object obj) => new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

       /// <summary>
       /// GET /DELETE calls handler
       /// </summary>
       /// <param name="action">Enum</param>
       /// <param name="controller">Enum</param>
       /// <param name="id"></param>
       /// <exception cref="InvalidActionException"/>
       /// <returns>JSON response</returns>
        public static async TT.Task<string> MakeApiCall(Action action, Controller controller, string id = "")
        {
            HttpResponseMessage res;
            if (action == Action.GET)
                res = await client.GetAsync($"{controller}/{id}");
            else if (action == Action.DELETE)
                res = await client.DeleteAsync($"{controller}/{id}");
            else throw new InvalidActionException("Provide either GET or DELETE actions");

            if (res.IsSuccessStatusCode) return await res.Content.ReadAsStringAsync();
            else MessageBox.Show($"Status Code: {res.StatusCode}\n{res.ReasonPhrase}"); return "";
        }

        /// <summary>
        /// PUT / POST calls handler
        /// </summary>
        /// <param name="action">Enum</param>
        /// <param name="controller">Enum</param>
        /// <param name="id"></param>
        /// <param name="data">Data</param>
        /// <exception cref="InvalidActionException"/>
        /// <returns>JSON response</returns>
        public static async TT.Task<string> MakeApiCall(Action action, Controller controller, object data, string id = "")
        {
            HttpResponseMessage res;
            if (action == Action.POST)
                res = await client.PostAsync($"{controller}/{id}", GetStringContent(data));
            else if (action == Action.PUT)
                res = await client.PutAsync($"{controller}/{id}", GetStringContent(data));
            else throw new InvalidActionException("Provide either GET or DELETE actions");

            if (res.IsSuccessStatusCode) return await res.Content.ReadAsStringAsync();
            else MessageBox.Show($"Status Code: {res.StatusCode}\n{res.ReasonPhrase}"); return "";
        }

        public static void ShowResMessage(HttpResponseMessage res)
        {
            MessageBox.Show($"Status Code: {res.StatusCode}\n{res.ReasonPhrase}");
        }


        public static void initClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         
            client.BaseAddress = new Uri(API_HOST);
        }

        public enum Controller
        {
            Addresses,
            Auth,
            Home,
            Issues,
            Paiment,
            Projects,
            Roles,
            Stacks,
            Tasks,
            Teams,
            TeamStacks,
            Test,
            Users
        };
        public enum Action
        {
            GET,
            POST,
            PUT,
            DELETE
        };

        class InvalidActionException : Exception
        {
            public InvalidActionException(string message) : base(message) { }
        }
    }
}
