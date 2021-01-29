using System;
using System.IO;
using System.Linq;
using System.Net;

using FACTS.GenericBooking.Common.Models.Domain;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace FACTS.GenericBooking.Api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ApiBadRequest(ApiMessage message)
        {
            return BadRequest(new
            {
                Errors = message.Errors.Select(x => new
                {
                    //x.Field,
                    x.Message
                }),
                Status = new
                {
                    Code      = (int) HttpStatusCode.BadRequest,
                    Name      = HttpStatusCode.BadRequest.ToString(),
                    Timestamp = DateTime.UtcNow.ToString("s")
                }
            });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ApiOk()
        {
            return Ok(new
            {
                Data = new { },
                Status = new
                {
                    Code      = (int) HttpStatusCode.OK,
                    Name      = HttpStatusCode.OK.ToString(),
                    Timestamp = DateTime.UtcNow.ToString("s")
                }
            });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ApiOk(object result)
        {
            return Ok(new
            {
                Data = result ?? new { },
                Status = new
                {
                    Code      = (int) HttpStatusCode.OK,
                    Name      = HttpStatusCode.OK.ToString(),
                    Timestamp = DateTime.Now.ToString("s")
                }
            });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected void SaveToFile<T>(T result, string customFileName = "") where T : class
        {
        #if DEBUG
            string serviceResultClassName = result.GetType().Name;
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            string dir = @"d:\temp\FactsBookingApi";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fileName = $@"{dir}\{serviceResultClassName}-{customFileName}-{DateTime.Now:yyyMMdd-hhmmss}.json";
            using StreamWriter writer = new StreamWriter(fileName);
            writer.Write(json);
            writer.Flush();
        #endif
        }
    }
}