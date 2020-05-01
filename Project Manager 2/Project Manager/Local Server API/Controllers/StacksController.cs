using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Local_Server_API.Models;

namespace Local_Server_API.Controllers
{
    [AuthorizaAttr]
    public class StacksController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        public IQueryable<Stack> GetStack()
        {
            return db.Stack;
        }

        [ResponseType(typeof(Stack))]
        public IHttpActionResult GetStack(int id)
        {
            Stack stack = db.Stack.Find(id);
            if (stack == null)
            {
                return NotFound();
            }

            return Ok(stack);
        }

        [AuthorizaAttr(new string[] { "Manager", "TeamLeader" })]
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

        [AuthorizaAttr(new string[] { "Manager", "TeamLeader" })]
        [ResponseType(typeof(Stack))]
        public IHttpActionResult PostStack(Stack stack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stack.Add(stack);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = stack.StackID }, stack);
        }

        [AuthorizaAttr("Manager")]
        [ResponseType(typeof(Stack))]
        public IHttpActionResult DeleteStack(int id)
        {
            Stack stack = db.Stack.Find(id);
            if (stack == null)
            {
                return NotFound();
            }

            db.Stack.Remove(stack);
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
            return db.Stack.Count(e => e.StackID == id) > 0;
        }
    }
}