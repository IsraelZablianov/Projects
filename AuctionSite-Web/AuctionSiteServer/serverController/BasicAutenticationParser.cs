using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace AuctionSiteServer.serverController
{
    public static class BasicAutenticationParser
    {
        public static BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
        }

        public static BasicAuthenticationIdentity ParseAuthorizationHeader(string credentials)
        {
            if (string.IsNullOrEmpty(credentials))
                return null;

            string[] headerParsed = credentials.Split(' ');

            if (headerParsed[0] != "Basic" || string.IsNullOrEmpty(headerParsed[1]))
                return null;

            var authHeader = Encoding.Default.GetString(Convert.FromBase64String(headerParsed[1]));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
        }
    }
}
