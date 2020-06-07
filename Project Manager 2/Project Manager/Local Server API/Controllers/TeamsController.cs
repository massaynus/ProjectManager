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
using Microsoft.Ajax.Utilities;

namespace Local_Server_API.Controllers
{
    public class TeamsController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthAttr]
        public IQueryable<Team> GetTeam()
        {
            return db.Teams;
        }

        [AuthAttr]
        [ResponseType(typeof(Team))]
        public IHttpActionResult GetTeam(int id)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        [AuthAttr(new string[] { Role.Manager, Role.TeamLeader })]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeam(int id, Team team)
        {
            var user = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.TeamID)
            {
                return BadRequest();
            } 

            if (user.Role1.RoleName == Role.TeamLeader)
            {
                if (user.Team != team.TeamID)
                {
                    return Unauthorized();
                }
            }
            
            var t = db.Teams.Where(T => T.TeamID == id).FirstOrDefault();
            if (t is null) BadRequest("Team doesn't exist");

            t.Name = team.Name;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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
        [ResponseType(typeof(Team))]
        public IHttpActionResult PostTeam(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teams.Add(team);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = team.TeamID }, team);
        }

        [AuthAttr(Role.Manager)]
        [ResponseType(typeof(Team))]
        public IHttpActionResult DeleteTeam(int id)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }

            db.Teams.Remove(team);
            db.SaveChanges();

            return Ok(team);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.TeamID == id) > 0;
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