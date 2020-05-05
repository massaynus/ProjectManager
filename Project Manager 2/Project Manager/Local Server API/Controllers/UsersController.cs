using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccess.Models;
using Local_Server_API.Models;

namespace Local_Server_API.Controllers
{
    public class UsersController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthorizaAttr(new string[] { Role.Manager, Role.TeamLeader })]
        public IEnumerable<User> GetUser()
        {
            var res = db.Users.Where(u => u.isAccountActive).ToList();
            return res;
        }

        [AuthorizaAttr]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Where(U => U.isAccountActive && U.UserID == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [AuthorizaAttr(new string[] { Role.Manager, Role.TeamLeader })]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

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

        [AuthorizaAttr(Role.Manager)]
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
                return BadRequest("-1");
            }

            user.isAccountActive = true;
            user.Password = AuthController.HashPassword(user.Password).ToString();

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        [AuthorizaAttr(Role.Manager)]
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

    }
}