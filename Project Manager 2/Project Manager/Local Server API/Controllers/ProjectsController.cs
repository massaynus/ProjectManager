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
using DataAccess.Models; using Local_Server_API.Models;

namespace Local_Server_API.Controllers
{
    public class ProjectsController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthAttr(new string[] { Role.Manager, Role.TeamLeader, Role.Client })]
        public ICollection<Project> GetProject()
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            List<Project> Projects = new List<Project>();

            switch (RequestingUser.Role1.RoleName)
            {
                case Role.Client:
                    Projects = db.Projects.Where(P => P.Owner == RequestingUser.UserID).ToList();
                    break;

                case Role.TeamLeader:
                    Projects = db.Projects.Where(P => (P.Team ?? 0) == (RequestingUser.Team ?? -1)).ToList();
                    break;
                case Role.Manager:
                    Projects = db.Projects.ToList();
                    break;
            }
            return Projects;
        }

        [AuthAttr(new string[] { Role.Manager, Role.TeamLeader, Role.Client })]
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            List<Project> Projects = new List<Project>();

            switch (RequestingUser.Role1.RoleName)
            {
                case Role.Client:
                    Projects = db.Projects.Where(P => P.Owner == RequestingUser.UserID).ToList();
                    break;

                case Role.TeamLeader:
                    Projects = db.Projects.Where(P => (P.Team ?? 0) == (RequestingUser.Team ?? -1)).ToList();
                    break;
                case Role.Manager:
                    Projects = db.Projects.ToList();
                    break;
            }


            Project project = Projects.Where(P => P.ProjectID == id).FirstOrDefault();

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [AuthAttr(Role.Manager)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectID)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = project.ProjectID }, project);
        }

        [AuthAttr(Role.Manager)]
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectID == id) > 0;
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