﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Local_Server_API.Models
{
    public class AuthorizaAttr : AuthorizationFilterAttribute
    {
        //TODO: check for ownership
        //TODO: add allowAll

        private string[] Role = new string[] { };
        private bool AllowAll;

        public AuthorizaAttr(bool AllowAll = false) : base() { this.AllowAll = AllowAll; }

        public AuthorizaAttr(string Role, bool AllowAll = false) : this(AllowAll)
        {
            this.Role = new string[] { Role };
        }

        public AuthorizaAttr(string[] Role, bool AllowAll = false) : this(AllowAll)
        {
            this.Role = Role;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers
                    .Authorization.Parameter;

                var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                    Convert.FromBase64String(authToken));

                var arrUserNameandPassword = decodeauthToken.Split(':');

                if (IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(arrUserNameandPassword[0]), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request
                 .CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        private bool IsAuthorizedUser(string UserName, string Password)
        {
            if (AllowAll) return true;

            using (var db = new DataAccess.Models.Local_DB_Model())
            {
                string hash = Controllers.AuthController.HashPassword(Password).ToString();
                var user = db.Users.Where(U => U.UserName == UserName && U.Password == hash).FirstOrDefault();
                if (user != null)
                {
                    return Role.Length > 0 ? Role.Contains(user.Role1.RoleName) : true;
                }
            }
            
            return false;
        }
    }
}