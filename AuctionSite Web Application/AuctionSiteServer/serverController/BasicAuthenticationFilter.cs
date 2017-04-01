using AuctionSiteServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AuctionSiteServer.serverController
{
    /// <summary>
    /// Generic Basic Authentication filter that checks for basic authentication
    /// headers and challenges for authentication if no authentication is provided
    /// Sets the Thread Principle with a GenericAuthenticationPrincipal.
    /// 
    /// You can override the OnAuthorize method for custom auth logic that
    /// might be application specific.    
    /// </summary>
    /// <remarks>Always remember that Basic Authentication passes username and passwords
    /// from client to server in plain text, so make sure SSL is used with basic auth
    /// to encode the Authorization header on all requests (not just the login).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BasicAuthenticationFilter : AuthorizationFilterAttribute
    {
        bool Active = true;
        bool LoginMode = false;
        private DBManager _DBManager = new DBManager();

        public BasicAuthenticationFilter()
        { }

        /// <summary>
        /// Overriden constructor to allow explicit disabling of this
        /// filter's behavior. Pass false to disable (same as no filter
        /// but declarative)
        /// </summary>
        /// <param name="active"></param>
        public BasicAuthenticationFilter(bool active, bool loginMode)
        {
            Active = active;
            LoginMode = loginMode;
        }

        /// <summary>
        /// Override to Web API filter method to handle Basic Auth check
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Active)
            {
                var identity = ParseAuthorizationHeader(actionContext);

                if (identity != null && LoginMode && OnAuthorizeUser(identity.Name, identity.Password, actionContext))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK);
                    return;
                }
                else if (LoginMode)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return;
                }

                if (identity == null)
                {
                    Challenge(actionContext);
                    return;
                }

                if (!OnAuthorizeUser(identity.Name, identity.Password, actionContext))
                {
                    Challenge(actionContext);
                    return;
                }

                var principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;
                base.OnAuthorization(actionContext);
            }
        }

        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            var autorized = false;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                autorized = false;
            }
            else if (_DBManager.IsUserExists(username, password))
            {
                autorized = true;
            }

            return autorized;
        }

        /// <summary>
        /// Parses the Authorization header and creates user credentials
        /// </summary>
        /// <param name="actionContext"></param>
        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            return BasicAutenticationParser.ParseAuthorizationHeader(actionContext);
        }

        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionContext"></param>
        private void Challenge(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }
    }
}