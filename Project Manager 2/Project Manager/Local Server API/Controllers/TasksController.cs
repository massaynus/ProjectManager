﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
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
    public class TasksController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthAttr]
        public ICollection<Task> GetTask()
        {
            var user = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            List<Task> Tasks = new List<Task>();

            switch (user.Role1.RoleName)
            {
                case Role.Manager:
                    Tasks = db.Tasks.ToList();
                    break;

                case Role.TeamLeader:
                    user.Team1.Projects.ToList()
                        .ForEach(P => Tasks.AddRange(P.Tasks));

                    break;

                case Role.Member:
                    user.Team1.Projects.ToList()
                        .ForEach(P => Tasks.AddRange(P.Tasks.Where(T => T.isComplete == false)));

                    break;

                case Role.Client:
                    db.Projects.Where(P => P.Owner == user.UserID).ToList()
                        .ForEach(P => Tasks.AddRange(P.Tasks));

                    break;
            }



            return Tasks;
        }

        [AuthAttr]
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            var user = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            List<Task> Tasks = new List<Task>();

            switch (user.Role1.RoleName)
            {
                case Role.Manager:
                    Tasks = db.Tasks.ToList();
                    break;

                case Role.TeamLeader:
                case Role.Member:
                    user.Team1.Projects.ToList()
                        .ForEach(P => Tasks.AddRange(P.Tasks));

                    break;

                case Role.Client:

                    db.Projects.Where(P => P.Owner == user.UserID).ToList()
                        .ForEach(P => Tasks.AddRange(P.Tasks));

                    break;
            }

            Task task = Tasks.Where(T => T.TaskID == id).FirstOrDefault();
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [AuthAttr(Role.TeamLeader)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, Task task)
        {
            var user = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            List<Task> Tasks = new List<Task>();

            user.Team1.Projects.ToList()
                .ForEach(P => Tasks.AddRange(P.Tasks));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.TaskID)
            {
                return BadRequest();
            }

            var dbTask = Tasks.Where(T => T.TaskID == id).FirstOrDefault();
            if (dbTask is null || dbTask.Project1.Team != user.Team)
            {
                return Unauthorized();
            }
            dbTask.Name = task.Name;
            dbTask.Description = task.Description;
            dbTask.Priority = task.Priority;
            dbTask.Difficulty = task.Difficulty;
            dbTask.DeadLine = task.DeadLine;
            dbTask.Stack = task.Stack;
            dbTask.isBooked = false;
            dbTask.isComplete = false;


            db.Entry(dbTask).State = EntityState.Modified;

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

        [AuthAttr]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult BookTask(int id = -1)
        {
            var Booker = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            var task = db.Tasks.Where(T => T.TaskID == id).FirstOrDefault();
            
            if (task != null)
            {
                if (task.isBooked == true) return BadRequest("This task is already booked");

                task.isBooked = true;
                task.DoneBy = Booker.UserID;

                try
                {
                    db.SaveChanges();
                }
                catch (EntityException ex)
                {
                    InternalServerError(ex);
                }
                return Ok();
            }

            return BadRequest("Task not found");
        }

        [AuthAttr]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult CompleteTask(int id = -1)
        {
            var Booker = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            var task = db.Tasks.Where(T => T.TaskID == id).FirstOrDefault();

            if (task != null)
            {
                if (task.DoneBy != Booker.UserID) BadRequest("You can only complete tasks you have booked");
                
                task.isComplete = true;

                try
                {
                    db.SaveChanges();
                }
                catch (EntityException ex)
                {
                    InternalServerError(ex);
                }
                return Ok();
            }

            return BadRequest("Task not found");
        }

        [AuthAttr(Role.TeamLeader)]
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            var user = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (db.Projects.Where(P => P.ProjectID == task.Project).FirstOrDefault().Team != user.Team)
            {
                return Unauthorized();
            }

            if (task.isBooked is null) task.isBooked = false;
            if (task.isComplete is null) task.isComplete = false;

            db.Tasks.Add(task);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.TaskID }, task);
        }

        [AuthAttr(Role.TeamLeader)]
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            var user = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            List<Task> Tasks = new List<Task>();


            user.Team1.Projects.ToList()
                .ForEach(P => Tasks.AddRange(P.Tasks));

            Task task = Tasks.Where(T => T.TaskID == id).FirstOrDefault();
            if (task == null)
            {
                return NotFound();
            }

            if(task.Project1.Team1 != user.Team1)
            {
                Unauthorized();
            }

            if ((task.isBooked ?? false) || (task.isComplete ?? false))
            {
                BadRequest("You Can't delete a booked or a completed Task");
            }

            db.Tasks.Remove(task);
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
            return db.Tasks.Count(e => e.TaskID == id) > 0;
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