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
    public class PaimentsController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthorizaAttr(new string[] { Role.Manager, Role.Client})]
        public IQueryable<Paiments> GetPaiments()
        {
            return db.Paiments;
        }

        [AuthorizaAttr(new string[] { Role.Manager, Role.Client})]
        [ResponseType(typeof(Paiments))]
        public IHttpActionResult GetPaiments(int id)
        {
            Paiments paiments = db.Paiments.Find(id);
            if (paiments == null)
            {
                return NotFound();
            }

            return Ok(paiments);
        }

        [AuthorizaAttr(Role.Manager)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPaiments(int id, Paiments paiments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paiments.PaymentID)
            {
                return BadRequest();
            }

            db.Entry(paiments).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaimentsExists(id))
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

        [AuthorizaAttr(Role.Manager)]
        [ResponseType(typeof(Paiments))]
        public IHttpActionResult PostPaiments(Paiments paiments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Paiments.Add(paiments);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = paiments.PaymentID }, paiments);
        }

        [AuthorizaAttr(Role.Manager)]
        [ResponseType(typeof(Paiments))]
        public IHttpActionResult DeletePaiments(int id)
        {
            Paiments paiments = db.Paiments.Find(id);
            if (paiments == null)
            {
                return NotFound();
            }

            db.Paiments.Remove(paiments);
            db.SaveChanges();

            return Ok(paiments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaimentsExists(int id)
        {
            return db.Paiments.Count(e => e.PaymentID == id) > 0;
        }
    }
}