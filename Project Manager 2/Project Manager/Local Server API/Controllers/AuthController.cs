using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

using DataAccess.Models;

namespace Local_Server_API.Controllers
{
    public class AuthController : ApiController
    {
        Local_DB_Model db = new Local_DB_Model();
        public class Creds
        {
            public string username { get; set; }
            public string password { get; set; }

            public override string ToString() => username + password;
        }
        /// <summary>
        /// Converts a string to an unsigned long using SHA1
        /// </summary>
        /// <param name="password">the string to convert</param>
        /// <returns>SHA1 password</returns>
        public static ulong HashPassword(string password)
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

        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult PostAuth([FromBody] Creds creds)
        {
            if (string.IsNullOrEmpty(creds.ToString())) return StatusCode(HttpStatusCode.BadRequest);
            string hash = HashPassword(creds.password).ToString();

            User user = db.Users.Where(U => U.UserName == creds.username && U.Password == hash).FirstOrDefault();
            
            if (user is null) return NotFound();
            
            return Ok(user);
        }
    }
}