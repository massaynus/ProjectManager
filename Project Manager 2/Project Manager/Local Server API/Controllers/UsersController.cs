using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccess.Models;
using Local_Server_API.Models;
using Microsoft.Ajax.Utilities;

namespace Local_Server_API.Controllers
{
    public class UsersController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        /// <summary>
        /// Get All the users
        /// </summary>
        /// <returns>
        /// Everything if you're a manager
        /// Team Members if you are a TeamLeader
        /// </returns>
        [AuthAttr(new string[] { Role.Manager, Role.TeamLeader })]
        public IEnumerable<User> GetUser()
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            string RequestingRole = RequestingUser?.Role1.RoleName;

            List<User> res = new List<User>();
            if (RequestingRole == Role.TeamLeader)
            {
                res = RequestingUser?.Team1?.Users.ToList();
            }

            else if (RequestingRole == Role.Manager)
            {
                res = db.Users.Where(u => u.Manager == RequestingUser.UserID)?.ToList();
            }

            return res;
        }

        [AuthAttr]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            User user = db.Users.Where(U => U.UserID == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [AuthAttr(Role.Manager)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            //RU: RequestingUser
            var RU = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest("ID and user.UserID don't match");
            }

            if (user.Team is null) user.Team1 = null;

            if (!string.IsNullOrEmpty(user.Password) && user.Password != "Don't change") user.Password = AuthController.HashPassword(user.Password);
            else user.Password = db.Users.Where(U => U.UserID == id).FirstOrDefault()?.Password;

            db.Users.AddOrUpdate(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [AuthAttr(Role.Manager)]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user is null)
            {
                return BadRequest("No data was supplied");
            }

            if (db.Users.Where(U => U.UserName == user.UserName).FirstOrDefault() != null)
            {
                return BadRequest("User name is taken");
            }

            user.isAccountActive = true;
            user.Password = AuthController.HashPassword(user.Password).ToString();

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        [AuthAttr(Role.Manager)]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }

        [NonAction]
        /// <summary>
        /// Gets the user based on the Auth header parameter
        /// </summary>
        /// <param name="AuthParameter"><code>ActionContext.Request.Headers.Authorization.Parameter</code></param>
        /// <returns></returns>
        public User GetUserFromAuthHeader(string AuthParameter)
        {
            string RequestingUID = Encoding.UTF8.GetString(Convert.FromBase64String(AuthParameter)).Split(':')[0];
            return db.Users.Where(U => U.UserName == RequestingUID).FirstOrDefault();
        }

    }
}