using System.IO;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.ExtensionMethods;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FACTS.GenericBooking.Common.Helpers
{
    public static class FileHelper
    {
        public static Task SaveFileAsync(string content, string baseFolder, string fileName)
        {
            if (content.IsJson())
            {
                content = JToken.Parse(content).ToString(Formatting.Indented);
            }

            string filePath = Path.Join(baseFolder, fileName);
            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }

            return File.WriteAllTextAsync(filePath, content);
        }
    }
}