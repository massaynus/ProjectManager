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
    public class TasksController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        public IQueryable<Task> GetTask()
        {
            return db.Task;
        }

        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [AuthorizaAttr(Role.TeamLeader)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.TaskID)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        [AuthorizaAttr(Role.TeamLeader)]
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            var req = RequestContext;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Task.Add(task);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.TaskID }, task);
        }

        [AuthorizaAttr(Role.TeamLeader)]
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            db.Task.Remove(task);
            db.SaveChanges();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Task.Count(e => e.TaskID == id) > 0;
        }
    }
}