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
using System.Web.UI.WebControls;
using DataAccess.Models; using Local_Server_API.Models;

namespace Local_Server_API.Controllers
{
    public class AddressesController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();
        [AuthAttr(Role.Manager)]
        public ICollection<Address> GetAddress()
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            return RequestingUser.Addresses;
        }

        [AuthAttr]
        [ResponseType(typeof(Address))]
        public IHttpActionResult GetAddress(int id)
        {
            var RequestingUser = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            Address address = RequestingUser.Addresses.Where(A => A.AddressID == id).FirstOrDefault();

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutAddress(int id, Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != address.AddressID)
            {
                return BadRequest();
            }

            if (GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter).Addresses.Where(A => A.AddressID == id).FirstOrDefault() == null)
            {
                return Unauthorized();
            }


            db.Entry(address).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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
        [ResponseType(typeof(Address))]
        public IHttpActionResult PostAddress(Address address)
        {
            var user = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (address.User.UserName != user.UserName) return Unauthorized();

            db.Addresses.Add(address);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = address.AddressID }, address);
        }

        [AuthAttr(Role.Manager)]
        [ResponseType(typeof(Address))]
        public IHttpActionResult DeleteAddress(int id)
        {
            var user = GetUserFromAuthHeader(ActionContext.Request.Headers.Authorization.Parameter);
            Address address = user.Addresses.Where(A => A.AddressID == id).FirstOrDefault();


            if (address == null)
            {
                return NotFound();
            }

            db.Addresses.Remove(address);
            db.SaveChanges();

            return Ok(address);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(int id)
        {
            return db.Addresses.Count(e => e.AddressID == id) > 0;
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