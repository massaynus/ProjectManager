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
    [AuthorizaAttr]
    public class PaimentController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        [AuthorizaAttr(new string[] { Role.Manager, Role.Client})]
        public IQueryable<Paiment> GetPaiment()
        {
            return db.Paiments;
        }

        [AuthorizaAttr(new string[] { Role.Manager, Role.Client})]
        [ResponseType(typeof(Paiment))]
        public IHttpActionResult GetPaiment(int id)
        {
            Paiment Paiment = db.Paiments.Find(id);
            if (Paiment == null)
            {
                return NotFound();
            }

            return Ok(Paiment);
        }

        [AuthorizaAttr(Role.Manager)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPaiment(int id, Paiment Paiment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Paiment.PaymentID)
            {
                return BadRequest();
            }

            db.Entry(Paiment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaimentExists(id))
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
        [ResponseType(typeof(Paiment))]
        public IHttpActionResult PostPaiment(Paiment Paiment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Paiments.Add(Paiment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Paiment.PaymentID }, Paiment);
        }

        [AuthorizaAttr(Role.Manager)]
        [ResponseType(typeof(Paiment))]
        public IHttpActionResult DeletePaiment(int id)
        {
            Paiment Paiment = db.Paiments.Find(id);
            if (Paiment == null)
            {
                return NotFound();
            }

            db.Paiments.Remove(Paiment);
            db.SaveChanges();

            return Ok(Paiment);
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
    }
}