using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using FACTS.GenericBooking.Common.Models.Domain;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace FACTS.GenericBooking.Common.Helpers
{
    public static class JsonHelpers
    {
        public static JsonSerializerSettings SerializerIndented { get; } = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling        = DateParseHandling.None,
            NullValueHandling        = NullValueHandling.Ignore,
            Formatting               = Formatting.Indented,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            },
        };
        public static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling        = DateParseHandling.None,
            NullValueHandling        = NullValueHandling.Ignore,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            },
        };
        public static JsonSerializerSettings SerializerSettingsDefault { get; } = new JsonSerializerSettings
        {
            ContractResolver  = new DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
        };

        public static Result<T> DeseraliseObject<T>(string order)
        {
            List<string> errors = new List<string>();
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                Error = (sender, errorArgs) =>
                {
                    errors.Add(errorArgs.ErrorContext.Error.Message);
                    errorArgs.ErrorContext.Handled = true;
                }
            };
            T deserializeObject = JsonConvert.DeserializeObject<T>(order, jsonSerializerSettings);
            return errors.Any() ? new Result<T>(string.Join('\n', errors)) : new Result<T>(deserializeObject);
        }
    }
}