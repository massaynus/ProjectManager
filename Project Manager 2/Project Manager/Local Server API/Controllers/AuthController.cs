using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace Local_Server_API.Controllers
{
    public class AuthController : ApiController
    {
        /// <summary>
        /// Converts a string to an unsigned long using SHA1
        /// </summary>
        /// <param name="password">the string to convert</param>
        /// <returns>SHA1 password</returns>
        private ulong HashPassword(string password)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] pwd = Encoding.UTF8.GetBytes(password);
                return BitConverter.ToUInt64(sha1.ComputeHash(pwd), 0);
            }
        }

        /// <summary>
        /// Gets the user ID if the auth is correct, otherwise it returns -1
        /// </summary>
        /// <param name="username">The UserName</param>
        /// <param name="password">The Password</param>
        /// <returns>The User ID</returns>
        [HttpPost]
        public int PostAuth(string username, string password)
        {
            using (var db = new Models.Local_DB_Model())
            {
                string hash = HashPassword(password).ToString();
                return db.User.Where(U => U.UserName == username && U.Password == hash).FirstOrDefault()?.UserID ?? -1;
            }
        }
        #region Default Code
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //} 
        #endregion
    }
}