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
using System.Web.UI;
using DataAccess.Models;
using Local_Server_API.Models;

namespace Local_Server_API.Controllers
{
    public class IssuesController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthAttr]
        public ICollection<Issue> GetIssue()
        {
            return GetTeamIssues(ActionContext.Request.Headers.Authorization.Parameter);
        }

        [AuthAttr]
        [ResponseType(typeof(Issue))]
        public IHttpActionResult GetIssue(int id)
        {
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return NotFound();
            }

            return Ok(issue);
        }

        [AuthAttr(Role.TeamLeader)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIssue(int id, Issue issue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != issue.IssueID)
            {
                return BadRequest();
            }

            var dbIssue = GetTeamIssues(ActionContext.Request.Headers.Authorization.Parameter).Where(I => I.IssueID == id).FirstOrDefault();

            if (dbIssue is null)
            {
                return Unauthorized();
            }

            dbIssue.isSolved = issue.isSolved;

            db.Entry(dbIssue).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(id))
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

        [AuthAttr(Role.Member)]
        [ResponseType(typeof(Issue))]
        public IHttpActionResult PostIssue(Issue issue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            if (issue.Issuer != requestingUser.UserID)
            {
                return BadRequest("Don't flag shit in someone else's name -_-");
            }

            var Task = db.Tasks.Where(T => T.TaskID == issue.Task).FirstOrDefault();
            if (Task != null)
            {
                if (Task.Project1.Team1.TeamID != requestingUser.Team)
                {
                    return Unauthorized();
                }
                db.Issues.Add(issue);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = issue.IssueID }, issue);
            }

            return BadRequest("Task doesn't exist");
        }

        [AuthAttr(Role.TeamLeader)]
        [ResponseType(typeof(Issue))]
        public IHttpActionResult DeleteIssue(int id)
        {
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return NotFound();
            }

            if (GetTeamIssues(ActionContext.Request.Headers.Authorization.Parameter).Where(I => I.IssueID == id).FirstOrDefault() is null)
            {
                return Unauthorized();
            }

            db.Issues.Remove(issue);
            db.SaveChanges();

            return Ok(issue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IssueExists(int id)
        {
            return db.Issues.Count(e => e.IssueID == id) > 0;
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

        private List<Issue> GetTeamIssues(string AuthParameter)
        {
            var RequestingUser = GetUserFromAuthHeader(AuthParameter);

            List<Issue> Issues = new List<Issue>();

            RequestingUser.Team1.Projects.ToList()
                .ForEach(p => p.Tasks.ToList()
                    .ForEach(t => t.Issues.ToList()
                                .ForEach(i => Issues.Add(i))
                            )
                        );

            return Issues;
        }
    }
}