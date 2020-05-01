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
    public class IssuesController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        public IQueryable<Issue> GetIssue()
        {
            return db.Issue;
        }

        [ResponseType(typeof(Issue))]
        public IHttpActionResult GetIssue(int id)
        {
            Issue issue = db.Issue.Find(id);
            if (issue == null)
            {
                return NotFound();
            }

            return Ok(issue);
        }

        [AuthorizaAttr(new string[] { Role.TeamLeader, Role.Member})]
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

            db.Entry(issue).State = EntityState.Modified;

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
        
        [AuthorizaAttr(Role.Member)]
        [ResponseType(typeof(Issue))]
        public IHttpActionResult PostIssue(Issue issue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Issue.Add(issue);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = issue.IssueID }, issue);
        }

        [AuthorizaAttr(Role.TeamLeader)]
        [ResponseType(typeof(Issue))]
        public IHttpActionResult DeleteIssue(int id)
        {
            Issue issue = db.Issue.Find(id);
            if (issue == null)
            {
                return NotFound();
            }

            db.Issue.Remove(issue);
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
            return db.Issue.Count(e => e.IssueID == id) > 0;
        }
    }
}