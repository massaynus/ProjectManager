using DataAccess.Models;
using Local_Server_API.Models;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace Local_Server_API.Controllers
{
    public class AuthController : ApiController
    {
        [NonAction]
        /// <summary>
        /// Converts a string to an unsigned long using SHA1
        /// </summary>
        /// <param name="password">the string to convert</param>
        /// <returns>SHA1 password</returns>
        public static string HashPassword(string password)
        {
            ulong Hash;
            using (var sha1 = SHA1.Create())
            {
                byte[] pwd = Encoding.UTF8.GetBytes(password);
                Hash = BitConverter.ToUInt64(sha1.ComputeHash(pwd), 0);
            }
            return Convert.ToBase64String(BitConverter.GetBytes(Hash));
        }

        [NonAction]
        /// <summary>
        /// Returns a user from a UID and Pwd
        /// </summary>
        public static User GetUser(string UserName, string Password)
        {
            string hash = HashPassword(Password);
            return new Local_DB_Model().Users.Where(U => U.UserName == UserName && U.Password == hash).FirstOrDefault();
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

            User user = GetUser(creds.username, creds.password);

            if (user is null) return NotFound();

            return Ok(user);
        }
    }
}