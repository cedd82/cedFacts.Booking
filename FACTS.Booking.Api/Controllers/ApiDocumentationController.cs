using System.IO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace FACTS.GenericBooking.Api.Controllers
{
    [Route("documentation")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiVersion( "1.0" )]
    public class ApiDocumentationController : ApiControllerBase
    {
        private readonly IFileProvider _fileProvider;

        public ApiDocumentationController(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        [HttpGet]
        public IActionResult Documentation()
        {
            string filePath = "ApiDocumentation.html";
            IFileInfo fileInfo = _fileProvider.GetFileInfo(filePath);
            Stream readStream = fileInfo.CreateReadStream();
            return File(readStream, "text/html");
        }
    }
}