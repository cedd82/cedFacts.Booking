using System.Collections.Generic;
using System.Linq;

using FACTS.GenericBooking.Common.Constants;

namespace FACTS.GenericBooking.Common.Models.Domain
{
    public class ApiMessage
    {
        public ApiMessage(string message, int level = MessageLevel.Error)
        {
            Errors = new List<ModelErrorDto>
            {
                new ModelErrorDto(message)
                {
                    Message = message,
                }
            };
            Level = level;
        }

        public List<ModelErrorDto> Errors { get; set; }
        public int Level { get; }

        public override string ToString()
        {
            return string.Join(",", Errors.Select(x => x.Message));
        }
    }
}