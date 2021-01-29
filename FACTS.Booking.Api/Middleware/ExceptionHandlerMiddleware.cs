using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Configuration;
using FACTS.GenericBooking.Common.Email;
using FACTS.GenericBooking.Common.Helpers;
using FACTS.GenericBooking.Common.Models;
using FACTS.GenericBooking.Common.Models.Domain.Messages;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using NLog;

namespace FACTS.GenericBooking.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RequestDelegate _next;
        private const int OneHundredKb = 128000;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,
                                 IEmailService emailService,
                                 IWebHostEnvironment hostingEnvironment,
                                 IHostApplicationLifetime appLifetime,
                                 CommonAppSettings commonAppSettings,
                                 IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, emailService, appLifetime, httpContextAccessor, hostingEnvironment, commonAppSettings);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context,
                                                Exception exception,
                                                IEmailService emailService,
                                                IHostApplicationLifetime appLifetime,
                                                IHttpContextAccessor httpContextAccessor,
                                                IWebHostEnvironment hostingEnvironment,
                                                CommonAppSettings commonAppSettings)
        {
            Logger.Error(exception, $"Unhandled Exception message: {exception.Message}");
            Type exceptionType = exception.GetType();
            Exception innerException = exception.InnerException;
            Type innerExceptionType = innerException?.GetType();
            HttpRequest httpRequest = httpContextAccessor.HttpContext.Request;
            bool? isAuthenticated = context.User.Identity?.IsAuthenticated;
            string userCode = null;
            if (isAuthenticated == true)
            {
                userCode = context.User.Identity?.Name;
            }

            string requestBody = string.Empty;
            if (commonAppSettings.CapturePostRequestBodyOnError && httpRequest.Method == "POST")
            {
                requestBody = await GetRequestBodyAsync(context);
            }

            string body = $@"{Environment.NewLine}Request: {httpRequest.Method} {httpRequest.Scheme}://{httpRequest.Host}{httpRequest.Path}{httpRequest.QueryString}
                            IsAuthenticated: {isAuthenticated}
                            RequestBody:{Environment.NewLine}{requestBody}{Environment.NewLine}
                            UserCode: {userCode}
                            ExceptionType: {exceptionType.Name}
                            InnerExceptionTypeIfAny: {innerExceptionType?.Name}
                            Exception: {exception}";
            string subject = $"Unhandled Exception caught in global exception handler {exception.Message}";
            emailService.SendErrorEmail(subject, body);
            
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode  = (int) HttpStatusCode.InternalServerError;
            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Errors = new List<ModelErrorDto>
                {
                    new ModelErrorDto(ErrorMessages.ApiException)
                },
                Status = new
                {
                    Code      = (int) HttpStatusCode.InternalServerError,
                    Name      = HttpStatusCode.InternalServerError.ToString(),
                    Timestamp = DateTime.UtcNow.ToString("s")
                }
            }, JsonHelpers.SerializerSettings)).ConfigureAwait(false);
        }
        
        private static async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            // protect api against failure on large byte size payloads, known issue where model binding fails where req > 64kb
            // https://stackoverflow.com/questions/49672813/request-with-size-over-64kb-fails-to-complete-asp-net-core-web-api
            if (context.Request.Body.Length > OneHundredKb)
            {
                return $"body size {context.Request.Body.Length} is larger than 128KB; too large to seralise.";
            }

            await using MemoryStream stream = new MemoryStream();
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            await context.Request.Body.CopyToAsync(stream);
            string requestBody = Encoding.UTF8.GetString(stream.ToArray());
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            return requestBody;
        }
    }
}