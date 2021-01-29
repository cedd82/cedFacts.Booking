using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Configuration;

using Microsoft.AspNetCore.Http;

using NLog;

namespace FACTS.GenericBooking.Api.Middleware
{
    public class SecureSwaggerBasicAuthMiddleware
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly List<string> WhiteListedExtensions = new List<string> {".css", ".js", ".png", ".gif", ".jpg", ".jpeg"};
        private readonly RequestDelegate _next;

        public SecureSwaggerBasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppSecrets config)
        {
            if (!config.SwaggerBasicAuthIsEnabled)
            {
                await _next.Invoke(context);
                return;
            }

            string uriComponent = context.Request.Path.ToUriComponent();

            if (!uriComponent.Contains("swagger"))
            {
                await _next.Invoke(context);
                return;
            }

            if (WhiteListedExtensions.Any(extension => uriComponent.EndsWith(extension)))
            {
                await _next.Invoke(context);
                return;
            }

            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                //Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int separatorIndex = usernamePassword.IndexOf(':');
                string username = usernamePassword.Substring(0, separatorIndex);
                string password = usernamePassword.Substring(separatorIndex + 1);

                if (username == config.SwaggerBasicAuthUserName && password == config.SwaggerBasicAuthPassword)
                {
                    await _next.Invoke(context);
                    return;
                }
            }

            // Authorization failed
            context.Response.Headers.Add("WWW-Authenticate", "Basic ");
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        }

        public bool IsLocalRequest(HttpContext context)
        {
            //Handle running using the Microsoft.AspNetCore.TestHost and the site being run entirely locally in memory without an actual TCP/IP connection
            if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
                return true;
            if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
                return true;
            if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
                return true;
            return false;
        }
    }
}