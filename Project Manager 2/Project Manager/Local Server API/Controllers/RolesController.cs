using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Local_Server_API.Models;

namespace Local_Server_API.Controllers
{
    public class RolesController : ApiController
    {
        private Local_DB_Model db = new Local_DB_Model();

        public IQueryable<Role> GetRole()
        {
            return db.Role;
        }

        [ResponseType(typeof(Role))]
        public async Task<IHttpActionResult> GetRole(int id)
        {
            Role role = await db.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}