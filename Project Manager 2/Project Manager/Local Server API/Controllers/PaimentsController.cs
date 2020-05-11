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
    public class PaimentController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthAttr(new string[] { Role.Manager, Role.Client })]
        public IQueryable<Paiment> GetPaiment()
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);

            return db.Paiments.Where(P => P.SenderID == RequestingUser.UserID || P.RecieverID == RequestingUser.UserID);
        }

        [AuthAttr(new string[] { Role.Manager, Role.Client })]
        [ResponseType(typeof(Paiment))]
        public IHttpActionResult GetPaiment(int id)
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            var payments = db.Paiments.Where(P => P.SenderID == RequestingUser.UserID || P.RecieverID == RequestingUser.UserID);


            Paiment Paiment = payments.Where(P => P.PaymentID == id).FirstOrDefault();

            if (Paiment == null)
            {
                return NotFound();
            }

            return Ok(Paiment);
        }

        [AuthAttr(new string[] { Role.Manager, Role.Client })]
        [ResponseType(typeof(Paiment))]
        public IHttpActionResult PostPaiment(Paiment Paiment)
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Paiment.SenderID != RequestingUser.UserID && Paiment.SenderFullName != (RequestingUser.LastName + " " + RequestingUser.FirstName))
            {
                return Unauthorized();
            }

            db.Paiments.Add(Paiment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Paiment.PaymentID }, Paiment);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaimentExists(int id)
        {
            return db.Paiments.Count(e => e.PaymentID == id) > 0;
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