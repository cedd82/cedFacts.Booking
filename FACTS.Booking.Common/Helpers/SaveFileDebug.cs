using System.IO;

using FACTS.GenericBooking.Common.ExtensionMethods;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FACTS.GenericBooking.Common.Helpers
{
    public static class SaveFileDebug
    {
        private const string BaseFolder = @"c:\temp\FactsBooking";
        public static void SaveFile(string content, string fileName)
        {
        #if DEBUG
            if (content.IsJson())
                content = JToken.Parse(content).ToString(Formatting.Indented);
            string filePath = Path.Join(BaseFolder, fileName);
            if (!Directory.Exists(BaseFolder))
                Directory.CreateDirectory(BaseFolder);
            File.WriteAllText(filePath, content);
        #endif
        }

        public static void SaveFile<T>(T content, string fileName)
        {
        #if DEBUG
            string json = JsonConvert.SerializeObject(content, JsonHelpers.SerializerIndented);
            string filePath = Path.Join(BaseFolder, fileName);
            if (!Directory.Exists(BaseFolder))
                Directory.CreateDirectory(BaseFolder);
            File.WriteAllText(filePath, json);
        #endif
        }
    }
}
