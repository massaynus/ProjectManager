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
using DataAccess.Models; using Local_Server_API.Models;

namespace Local_Server_API.Controllers
{
    public class TeamStacksController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        public IQueryable<TeamStack> GetTeamStack(int TeamId)
        {
            return db.TeamStacks.Where(TS => TS.Team == TeamId);
        }

        [AuthorizaAttr(Role.Manager)]
        [ResponseType(typeof(TeamStack))]
        public IHttpActionResult PostTeamStack(TeamStack teamStack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TeamStacks.Add(teamStack);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = teamStack.Num }, teamStack);
        }

        [AuthorizaAttr(Role.Manager)]
        [ResponseType(typeof(TeamStack))]
        public IHttpActionResult DeleteTeamStack(int id)
        {
            TeamStack teamStack = db.TeamStacks.Find(id);
            if (teamStack == null)
            {
                return NotFound();
            }

            db.TeamStacks.Remove(teamStack);
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
            return db.TeamStacks.Count(e => e.Num == id) > 0;
        }
    }
}