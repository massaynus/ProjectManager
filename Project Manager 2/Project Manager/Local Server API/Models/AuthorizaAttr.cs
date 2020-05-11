using DataAccess.Models;
using Local_Server_API.Controllers;
using System;
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
        private string[] Role = new string[] { };
        private string principaleRole = string.Empty;
        private bool AllowAll;

        public AuthorizaAttr(bool AllowAll = false) : base()
        {
            this.AllowAll = AllowAll;
        }

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

                if (IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1], actionContext))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(arrUserNameandPassword[0]), new string[] { principaleRole });
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

        private bool IsAuthorizedUser(string UserName, string Password, HttpActionContext actionContext)
        {
            if (AllowAll) return true;

            var user = AuthController.GetUser(UserName, Password);

            if (user != null)
            {
                LogAction(user, actionContext);

                if (Role.Length > 0)
                {
                    if (Role.Contains(user.Role1.RoleName))
                    {
                        principaleRole = user.Role1.RoleName;
                        return true;
                    }
                }
                else return true;
            }

            return false;
        }

        private void LogAction(User user, HttpActionContext context)
        {
            using (ActionLogContext db = new ActionLogContext())
            {
                ActionLog actionLog = new ActionLog();

                actionLog.UserName = user.UserName;
                actionLog.UserFullName = $"{user.LastName} {user.FirstName}";
                actionLog.ActionName = $"{context.ActionDescriptor.ControllerDescriptor.ControllerName}/{context.ActionDescriptor.ActionName}";
                actionLog.ActionMethod = context.Request.Method.Method;
                actionLog.RequestDate = DateTime.Now;

                actionLog.ActionDATA += "FromBody:\n\t";
                string BodyData = context.Request.Content.ReadAsStringAsync().Result;

                actionLog.ActionDATA += BodyData.Contains("Password") ? "Contained sensitive data!" : BodyData;

                actionLog.ActionDATA += "\n\nFromURI:\n";
                var URIParams = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
                URIParams.AllKeys
                    .ToList()
                    .ForEach(Key => actionLog.ActionDATA += $"\n\t{Key}:\t{URIParams[Key]}");

                var RouteParamsArr = context.Request.RequestUri.AbsolutePath.Split('/');
                if (RouteParamsArr.Length == 4)
                    actionLog.ActionDATA += "\n\nFromRoute:\t" + RouteParamsArr[3];

                if (string.IsNullOrEmpty(actionLog.ActionDATA))
                    actionLog.ActionDATA = "NO DATA";


                db.ActionLogs.Add(actionLog);

                db.SaveChanges();
            }
        }
    }
}