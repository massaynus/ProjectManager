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
    public class TeamStacksController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        // GET: api/TeamStacks
        public IQueryable<TeamStack> GetTeamStack()
        {
            return db.TeamStack;
        }

        // GET: api/TeamStacks/5
        [ResponseType(typeof(TeamStack))]
        public IHttpActionResult GetTeamStack(int id)
        {
            TeamStack teamStack = db.TeamStack.Find(id);
            if (teamStack == null)
            {
                return NotFound();
            }

            return Ok(teamStack);
        }

        // PUT: api/TeamStacks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeamStack(int id, TeamStack teamStack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teamStack.Num)
            {
                return BadRequest();
            }

            db.Entry(teamStack).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamStackExists(id))
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

        // POST: api/TeamStacks
        [ResponseType(typeof(TeamStack))]
        public IHttpActionResult PostTeamStack(TeamStack teamStack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TeamStack.Add(teamStack);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = teamStack.Num }, teamStack);
        }

        // DELETE: api/TeamStacks/5
        [ResponseType(typeof(TeamStack))]
        public IHttpActionResult DeleteTeamStack(int id)
        {
            TeamStack teamStack = db.TeamStack.Find(id);
            if (teamStack == null)
            {
                return NotFound();
            }

            db.TeamStack.Remove(teamStack);
            db.SaveChanges();

            return Ok(teamStack);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamStackExists(int id)
        {
            return db.TeamStack.Count(e => e.Num == id) > 0;
        }
    }
}