using Microsoft.AspNetCore.Builder;

namespace FACTS.GenericBooking.Api.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseEnableRequestBuffering(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EnableRequestBufferingMiddleware>();
        }

        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}