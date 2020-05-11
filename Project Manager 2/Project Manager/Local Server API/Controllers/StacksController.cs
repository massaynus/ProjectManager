using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccess.Models;
using Local_Server_API.Models;

namespace Local_Server_API.Controllers
{
    public class StacksController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthAttr]
        public IQueryable<Stack> GetStack()
        {
            return db.Stacks;
        }

        [AuthAttr]
        [ResponseType(typeof(Stack))]
        public IHttpActionResult GetStack(int id)
        {
            Stack stack = db.Stacks.Find(id);
            if (stack == null)
            {
                return NotFound();
            }

            return Ok(stack);
        }

        [AuthAttr(new string[] { Role.Manager, Role.TeamLeader })]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStack(int id, Stack stack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stack.StackID)
            {
                return BadRequest();
            }

            db.Entry(stack).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StackExists(id))
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

        [AuthAttr(new string[] { Role.Manager, Role.TeamLeader })]
        [ResponseType(typeof(Stack))]
        public IHttpActionResult PostStack(Stack stack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stacks.Add(stack);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = stack.StackID }, stack);
        }

        [AuthAttr(Role.Manager)]
        [ResponseType(typeof(Stack))]
        public IHttpActionResult DeleteStack(int id)
        {
            Stack stack = db.Stacks.Find(id);
            if (stack == null)
            {
                return NotFound();
            }

            db.Stacks.Remove(stack);
            db.SaveChanges();

            return Ok(stack);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StackExists(int id)
        {
            return db.Stacks.Count(e => e.StackID == id) > 0;
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