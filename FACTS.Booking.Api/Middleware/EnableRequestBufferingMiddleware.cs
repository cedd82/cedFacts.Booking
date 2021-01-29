using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace FACTS.GenericBooking.Api.Middleware
{
    public class EnableRequestBufferingMiddleware
    {
        private readonly RequestDelegate _next;

        public EnableRequestBufferingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            return _next(context);
        }
    }
}